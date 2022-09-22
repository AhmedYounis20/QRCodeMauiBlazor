using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using ZXing.Net.Maui;

namespace QRCodeMauiBlazor.Pages;

public partial class QRReader : ContentPage
{
    HttpClient client { get; set; }= new HttpClient();
    NavigationManager navigationManager;
    bool firstDetect = true;
	public QRReader(NavigationManager navigation)
	{
		InitializeComponent();
        navigationManager = navigation;
	}

    private void CameraBarcodeReaderView_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
            if (firstDetect)
            {
                firstDetect = !firstDetect;
                Dispatcher.Dispatch(async () =>
                 {
                string result = "";
                try
                {
                    Task.Run(async () => result = await client.GetFromJsonAsync<string>(e.Results[0].Value));
                    if ( (await DisplayAlert("QRCode Result", $"{result}", "tryagain","Done")))
                    {
                        firstDetect=!firstDetect;
                        navigationManager.NavigateTo("/");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    result = ex.Message;
                         if ((await DisplayAlert("QRCode Result", $"{result} {e.Results[0].Value}", "tryagain", "Done")))
                         {
                             firstDetect = !firstDetect;
                            
                         }

                     }

        });
            }
    }
    private void BackToMain(object sender,EventArgs args)
    {
        navigationManager.NavigateTo("/counter");
    }
}