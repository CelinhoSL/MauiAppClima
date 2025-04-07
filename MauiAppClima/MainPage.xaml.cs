using MauiAppClima.Models;
using MauiAppClima.Services;
using System.Net;
using System.Threading.Tasks;

namespace MauiAppClima
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txt_cidade.Text))
                {
                    var (t, statusCode) = await DataService.GetPrevisaoComStatus(txt_cidade.Text);

                    if (statusCode == HttpStatusCode.OK && t != null)
                    {
                        string dados_previsao = $"Latitude:  {t.lat}\n" +
                                                $"Longitude: {t.lon}\n" +
                                                $"Nascer do Sol: {t.sunrise}\n" +
                                                $"Por do Sol: {t.sunset}\n" +
                                                $"Temperatura Máxima: {t.temp_max}\n" +
                                                $"Temperatura Mínima: {t.temp_min}\n" +
                                                $"Descrição: {t.description}\n" +
                                                $"Visibilidade: {t.visibility}\n" +
                                                $"Velocidade do Vento: {t.speed}\n";

                        lbl_res.Text = dados_previsao;
                    }
                    else if (statusCode == HttpStatusCode.NotFound)
                    {
                        await DisplayAlert("Cidade não encontrada", "Verifique se digitou o nome corretamente.", "OK");
                        lbl_res.Text = string.Empty;
                    }
                    else
                    {
                        await DisplayAlert("Erro", $"Erro ao buscar os dados: {statusCode}", "OK");
                        lbl_res.Text = string.Empty;
                    }
                }
                else
                {
                    lbl_res.Text = "Digite uma cidade";
                }
            }
            catch (HttpRequestException)
            {
                await DisplayAlert("Sem conexão", "Você está sem conexão com a internet. Tente novamente quando estiver online.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro inesperado", ex.Message, "OK");
            }
        }
    };
}
//teste