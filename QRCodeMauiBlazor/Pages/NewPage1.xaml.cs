using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using ZXing.Net.Maui;

namespace QRCodeMauiBlazor.Pages;

public partial class NewPage1 : ContentPage
{
    HttpClient client { get; set; }= new HttpClient();
    NavigationManager navigationManager;
	public NewPage1(NavigationManager navigation)
	{
		InitializeComponent();
        navigationManager = navigation;
	}

    private void CameraBarcodeReaderView_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        Dispatcher.Dispatch( () =>
        {
            string result = "";
            try
            {
             Task.Run(async()=>result = await client.GetFromJsonAsync<string>(e.Results[0].Value));
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                result = ex.Message;
            }
             if(DisplayAlert("QRCode Result", $"{result} {e.Results[0].Value}", "Ok").IsCanceled)
            {
                navigationManager.NavigateTo("/");
            }

        });
    }
    private void BackToMain(object sender,EventArgs args)
    {
        navigationManager.NavigateTo("/counter");
    }
}