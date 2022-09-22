using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using ZXing.Net.Maui;

namespace QRCodeMauiBlazor.Pages;

public partial class QRReader : ContentPage
{
    HttpClient client { get; set; } = new HttpClient();
    bool firstDetect = true;
    public QRReader()
    {
        InitializeComponent();
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

                     if ((await DisplayAlert("QRCode Result", $"{result} {e.Results[0].Value}", "Done","try again")))
                         await Navigation.PopModalAsync();
                     else
                         firstDetect = !firstDetect;
                 }
                 catch (Exception ex)
                 {
                     Console.WriteLine(ex.Message);
                     result = ex.Message;
                     if ((await DisplayAlert("QRCode Result", $"{result} {e.Results[0].Value}", "Done", "try again")))
                         await Navigation.PopModalAsync();
                     else
                         firstDetect = !firstDetect;
                 }

             });
        }
    }
    async void BackToMainAsync(object sender, EventArgs args)
    {
        await Navigation.PopModalAsync();
    }
}