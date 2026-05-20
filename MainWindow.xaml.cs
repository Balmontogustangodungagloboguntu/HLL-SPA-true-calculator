using HLL_SPA_calculator.Models;
using HLL_SPA_calculator.Services;

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace HLL_SPA_calculator
{
    public partial class MainWindow : Window
    {
        private List<ArtilleryType> artilleryTypes;

        private InterpolationService interpolationService =
            new InterpolationService();


        private const int HOTKEY_ID = 9000;

        private const uint MOD_CONTROL = 0x0002;

        private const uint VK_O = 0x4F;

        public MainWindow()
        {
            InitializeComponent();


            LoadArtilleryData();


            ArtilleryComboBox.ItemsSource = artilleryTypes;

            ArtilleryComboBox.DisplayMemberPath = "Name";

            ArtilleryComboBox.SelectedIndex = 0;

            // START MINIMIZED (VISIBLE IN TASKBAR)
            this.WindowState = WindowState.Minimized;

            // REGISTER HOTKEY
            var helper = new WindowInteropHelper(this);

            helper.EnsureHandle();

            RegisterHotKey(
                helper.Handle,
                HOTKEY_ID,
                MOD_CONTROL,
                VK_O);

            HwndSource source =
                HwndSource.FromHwnd(helper.Handle);

            source.AddHook(HwndHook);

            // ENABLE DRAGGING
            this.MouseLeftButtonDown += (_, __) =>
            {
                DragMove();
            };

            // ESC CLOSES OVERLAY
            this.KeyDown += MainWindow_KeyDown;
        }

        private void LoadArtilleryData()
        {
            var dataService = new DataService();

            artilleryTypes = new List<ArtilleryType>
            {
                dataService.LoadArtillery("Data/BRUMMBÄRandPANZER3.json"),

                dataService.LoadArtillery("Data/M4A3andKV2.json"),

                dataService.LoadArtillery("Data/Bishop.json"),

                dataService.LoadArtillery("Data/Churchill AVRE.json"),
            };
        }
        

        private void Calculate_Click(
            object sender,
            RoutedEventArgs e)
        {
            try
            {
                double targetDistance =
                    double.Parse(DistanceTextBox.Text);

                double slopeOffset;

                double bias;

                double.TryParse(
                    SlopeOffsetTextBox.Text,
                    out slopeOffset);

                double.TryParse(
                    BiasTextBox.Text,
                    out bias);

                var selected =
                    (ArtilleryType)ArtilleryComboBox.SelectedItem;

                double finalMil;

                if (selected.Name == "Bishop")
                {
                    double firstResult =
                        (targetDistance + 29.14) / 4.49;

                    if (firstResult <= 267)
                    {
                        finalMil =
                            firstResult - slopeOffset + bias;
                    }
                    else
                    {
                        finalMil =
                            ((targetDistance - 82.5) / 4.27)
                            - slopeOffset + bias;
                    }
                }
                else
                {
                    double interpolated =
                        interpolationService.InterpolateMil(
                            selected.Table,
                            targetDistance);

                    finalMil =
                        interpolated - slopeOffset + bias;
                }

                ResultTextBlock.Text =
                    $"Final Mil: {finalMil:F2}";
            }
        }

        // GLOBAL HOTKEY HANDLER
        private IntPtr HwndHook(
            IntPtr hwnd,
            int msg,
            IntPtr wParam,
            IntPtr lParam,
            ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;

            if (msg == WM_HOTKEY)
            {
                int id = wParam.ToInt32();

                if (id == HOTKEY_ID)
                {
                    ToggleOverlay();

                    handled = true;
                }
            }

            return IntPtr.Zero;
        }

        // SHOW / HIDE OVERLAY
        private void ToggleOverlay()
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.WindowState = WindowState.Normal;

                this.Topmost = true;

                this.Activate();

                this.Focus();

                DistanceTextBox.Focus();

                return;
            }

            this.WindowState = WindowState.Minimized;
        }

        // ESC HIDES OVERLAY
        private void MainWindow_KeyDown(
            object sender,
            KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.WindowState = WindowState.Minimized;
            }
        }

        // WIN32 HOTKEY IMPORT
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(
            IntPtr hWnd,
            int id,
            uint fsModifiers,
            uint vk);
    }
}