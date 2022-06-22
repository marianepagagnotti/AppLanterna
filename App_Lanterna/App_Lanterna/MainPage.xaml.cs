using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Battery;
using Xamarin.Essentials;

namespace App_Lanterna
{
    public partial class MainPage : ContentPage
    {
        bool lanterna_ligada = false;
        public MainPage()
        {
            InitializeComponent();

            btnOnOff.Source = ImageSource.FromResource("App_Lanterna.Imagem.off.PNG");
           

            Carrega_Informacoes_Bateria();

        }


        private async void Carrega_Informacoes_Bateria()
        {
            try
            {
                /** 
                 * Verificando se o plugin é suportado pelo dispositivo. Caso contrário uma
                 * mensagem é exibida ao usuário avisando que as informações não estão disp
                 */

                if (CrossBattery.IsSupported)
                {
                    CrossBattery.Current.BatteryChanged -= Mudanca_Status_Bateria;
                    CrossBattery.Current.BatteryChanged += Mudanca_Status_Bateria;
                }
                else
                {
                    lbl_bateria_fraca.Text = "As informações sobre a bateria não esão disponíveis :(";
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ocorreu um erro: \n", ex.Message, "OK");
            }

        }

        private async void Mudanca_Status_Bateria(object sender, Plugin.Battery.Abstractions.BatteryChangedEventArgs e)
        {
            try
            {
                // Mostrando a carga restante da bateria em porcentagem.
                lbl_porcentagem_restante.Text = e.RemainingChargePercent.ToString() + "%";

                /** 
                 * Caso a bateria esteja fraca, mostra uma mensagem para o usuário
                 */
                if (e.IsLow)
                {
                    lbl_bateria_fraca.Text = "Atenção!!! A Bateria Está Fraca!!";
                }
                else
                {
                    lbl_bateria_fraca.Text = "";
                }

                /**
                 * Estrutura (escolha caso) para mostrar qual é o status da bateria: Carregando, 
                 * descarregando, etc. 
                 * O argumento e carrega as mudanças acontecidas na bateria, que pode ter sido colocada
                 * para carregar ou estar descarregando. Dentro desse argumento podemos acessar várias informações 
                 * como o satus da bateria, e tratamos para a mensagem ficar mais amigavel para o usuário
                 */

                switch (e.Status)
                {
                    case Plugin.Battery.Abstractions.BatteryStatus.Charging:
                        lbl_status.Text = "Carregando";
                        break;

                    case Plugin.Battery.Abstractions.BatteryStatus.Discharging:
                        lbl_status.Text = "Descarregando";
                        break;

                    case Plugin.Battery.Abstractions.BatteryStatus.Full:
                        lbl_status.Text = "Carregada";
                        break;

                    case Plugin.Battery.Abstractions.BatteryStatus.NotCharging:
                        lbl_status.Text = "Sem Carregar";
                        break;

                    case Plugin.Battery.Abstractions.BatteryStatus.Unknown:
                        lbl_status.Text = "Desconhecido";
                        break;
                }

                //Mostra a fonte de energia atual do dispositivo

                switch (e.PowerSource)
                {
                    case Plugin.Battery.Abstractions.PowerSource.Ac:
                        lbl_fonte_carregamento.Text = "Carregador";
                        break;

                    case Plugin.Battery.Abstractions.PowerSource.Battery:
                        lbl_fonte_carregamento.Text = "Bateria";
                        break;

                    case Plugin.Battery.Abstractions.PowerSource.Usb:
                        lbl_fonte_carregamento.Text = "USB";
                        break;

                    case Plugin.Battery.Abstractions.PowerSource.Wireless:
                        lbl_fonte_carregamento.Text = "Sem Fio";
                        break;

                    case Plugin.Battery.Abstractions.PowerSource.Other:
                        lbl_fonte_carregamento.Text = "Desconhecida";
                        break;

                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ocorreu um erro: \n", ex.Message, "OK");
            }
        }
        private async void btnOnOff_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!lanterna_ligada)
                {
                    lanterna_ligada = true;

                    btnOnOff.Source = ImageSource.FromResource("App_Lanterna.Imagem.on.PNG");

                    Vibration.Vibrate(TimeSpan.FromMilliseconds(250));

                    await Flashlight.TurnOnAsync();
                }
                else
                {
                    lanterna_ligada = false;

                    btnOnOff.Source = ImageSource.FromResource("App_Lanterna.Imagem.off.PNG");

                    Vibration.Vibrate(TimeSpan.FromMilliseconds(250));

                    await Flashlight.TurnOffAsync();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ocorreu um erro: \n", ex.Message, "OK");
            }
        }









    }
}
