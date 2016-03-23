using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace VisitorsLogMaintenance
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        string[,] value = new string[700, 2];
        bool[] index = new bool[700];
        bool yes;
        Windows.Storage.StorageFile file;
        private async void ss(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 700; i++)
            {
                index[i] = false;
                value[i, 0] = "";
                value[i, 1] = "";
            }
            file = null;
            yes = true;
            DateTime creationtime = DateTime.Now;
            string filename = creationtime.ToString("dd-MM-yyyy/HH-mm");
            //creating file
            Windows.Storage.Pickers.FileOpenPicker openPicker = new Windows.Storage.Pickers.FileOpenPicker();
            openPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            openPicker.ViewMode = Windows.Storage.Pickers.PickerViewMode.List;
            openPicker.FileTypeFilter.Clear();
            openPicker.FileTypeFilter.Add(".csv");
            file = await openPicker.PickSingleFileAsync();
            string heading = "ID/Name,In-Time,In-Date,Out-Time,Out-Date,Duration,Information," + filename;
            if (file != null)
            {
                await Windows.Storage.FileIO.AppendTextAsync(file, heading + Environment.NewLine);
            }
            if (file == null)
            {
                Windows.UI.Popups.MessageDialog messageDialog = new Windows.UI.Popups.MessageDialog("file not selected.", "Error Message:");
                await messageDialog.ShowAsync();
            }
        }


        private async void gi(object sender, RoutedEventArgs e)
        {
            DateTime intime;
            if (yes == true)
            {
                oid.Text = id.Text;
                intime = DateTime.Now;
                oit.Text = intime.ToString("hh-mm-ss tt");
                oidt.Text = intime.ToString("dd/MM/yyyy");

                for (int i = 0; i < 700; i++)
                {
                    if (index[i] == false)
                    {
                        value[i, 0] = id.Text;
                        value[i, 1] = intime.ToString("yyyy-MM-dd HH:mm:ss");
                        index[i] = true;
                        break;
                    }

                    if (file == null)
                    {
                        Windows.UI.Popups.MessageDialog messageDialog = new Windows.UI.Popups.MessageDialog("File not found.Start session and select appropriate file.", "Error Message:");
                        await messageDialog.ShowAsync();
                        break;
                    }
                }
            }
            else
            {
                Windows.UI.Popups.MessageDialog messageDialog = new Windows.UI.Popups.MessageDialog("Start session and select appropriate file to generate intime and enter entries.", "Error Message:");
                await messageDialog.ShowAsync();
            }
        }


        private async void go(object sender, RoutedEventArgs e)
        {
            DateTime outtime, intime;
            int i;
            oid.Text = "";
            oit.Text = "";
            oidt.Text = "";
            oot.Text = "";
            oodt.Text = "";
            odu.Text = "";
            oin.Text = "";
            if (yes == true && file!=null)
            {
                for (i = 0; i < 700; i++)
                {
                    if (index[i] == true)
                    {
                        if (value[i, 0] == id.Text)
                        {
                            intime = DateTime.ParseExact(value[i, 1], "yyyy-MM-dd HH:mm:ss",System.Globalization.CultureInfo.InvariantCulture);
                            oid.Text = id.Text;
                            oit.Text = intime.ToString("hh-mm-ss tt");
                            oidt.Text = intime.ToString("dd/MM/yyyy");
                            outtime = DateTime.Now;
                            oot.Text = outtime.ToString("hh-mm-ss tt");
                            oodt.Text = outtime.ToString("dd/MM/yyyy");
                            TimeSpan diff = outtime.Subtract(intime);
                            odu.Text = string.Format("{0:00}:{1:00}:{2:00}/{3:00}days", diff.Hours, diff.Minutes, diff.Seconds,diff.Days);
                            break;
                        }
                    }
                    if (i == 699)
                    {
                        Windows.UI.Popups.MessageDialog messageDialog = new Windows.UI.Popups.MessageDialog("Entry not found.Make sure you have entered correct Name/ID and the file is selected.", "Error Message:");
                        await messageDialog.ShowAsync();

                    }

                }
            }
            else
            {
                Windows.UI.Popups.MessageDialog messageDialog = new Windows.UI.Popups.MessageDialog("Start session and select appropriate file to enter and retrieve entries.", "Error Message:");
                await messageDialog.ShowAsync();
            }
        }

        private async void atl(object sender, RoutedEventArgs e)
        {
            if (yes == true)
            {
                if (file != null)
                {
                    string log = oid.Text + "," + oit.Text + "," + oidt.Text + "," + oot.Text + "," + oodt.Text + "," + odu.Text + "," + oin.Text;
                    await Windows.Storage.FileIO.AppendTextAsync(file, log + Environment.NewLine);

                    for (int i = 0; i < 700; i++)
                    {
                        if (index[i] == true)
                        {
                            if (value[i, 0] == oid.Text)
                            {
                                value[i, 0] = "";
                                value[i, 1] = "";
                                index[i] = false;
                                break;
                            }
                        }
                        
                    }
                }
                if (file == null)
                {
                    Windows.UI.Popups.MessageDialog messageDialog = new Windows.UI.Popups.MessageDialog("File not found.Start session and select appropriate file.", "Error Message:");
                    await messageDialog.ShowAsync();
                }  
            }
            else
            {
                Windows.UI.Popups.MessageDialog messageDialog = new Windows.UI.Popups.MessageDialog("Start session and select appropriate file to write log to the file.", "Error Message:");
                await messageDialog.ShowAsync();
            }
        }

        private async void es(object sender, RoutedEventArgs e)
        {
            if (yes == true && file!=null)
            {
                for (int i = 0; i < 700; i++)
                {
                    if(index[i]==true && file!=null)
                    {
                     DateTime intime;
                     intime = DateTime.ParseExact(value[i, 1], "yyyy-MM-dd HH:mm:ss",System.Globalization.CultureInfo.InvariantCulture);
                     string log =value[i,0]+","+intime.ToString("hh-mm-ss tt")+","+intime.ToString("dd/MM/yyyy ");
                     await Windows.Storage.FileIO.AppendTextAsync(file, log + Environment.NewLine);
                     index[i] = false;
                     value[i, 0] = "";
                     value[i, 1] = "";
                    } 
                }
                file = null;
                yes = false;
            }
            else
            {
                Windows.UI.Popups.MessageDialog messageDialog = new Windows.UI.Popups.MessageDialog("Start session,enter entries and then end session it when all entries are entered.", "Error Message:");
                await messageDialog.ShowAsync();
            }

        }
    }
}
