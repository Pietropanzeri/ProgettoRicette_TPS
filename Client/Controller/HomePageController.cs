﻿using Client.Model;
using Client.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace Client.Controller
{
    public partial class HomePageController : ObservableObject
    {
        public ObservableCollection<RicettaFoto> ListaNovità { get; set; } = new ObservableCollection<RicettaFoto>();

        [ObservableProperty]
        string error;

        List<Ricetta> listRicette = new List<Ricetta>();
        List<Foto> listFoto = new List<Foto>();

        //TODO: spostare baseUrl in modo tale che sia visibile da tutti
        string endpoint = "/ricette/novita/{indicePartenza}/{countRicette}";
        int indicePartenza = 0; 
        int countRicette = 10; 
        string apiUrl = string.Empty;
        public HomePageController()
        {
            apiUrl = $"{endpoint}"
                .Replace("{indicePartenza}", indicePartenza.ToString())
                .Replace("{countRicette}", countRicette.ToString());
        }
        [RelayCommand]
        public async Task Appearing()
        {
            ListaNovità.Clear();
            // TEST GRAFICA
            for (int i = 0;  15 > i;  i++)
            {
                ListaNovità.Add(new RicettaFoto()
                {
                    RicettaId = 1,
                    UtenteId = 101,
                    Nome = "Carbonara",
                    Preparazione = "Cuocere la pasta, preparare il ragù, comporre gli strati e infornare.",
                    Tempo = 60,
                    Difficolta = 3,
                    DataAggiunta = DateTime.Now,
                    Piatto = TipoPiatto.Primo,
                    URLFoto = "carbo.jpeg"
                });
            }
            
           
            /////////////////////////////////////////////
            await RichiestaHttp();
        }

        public async Task RichiestaHttp()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            HttpClient _client = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://192.168.1.56:5001")
            };

            List<Ricetta> content = new List<Ricetta>();

            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = await _client.GetAsync("/ricette/novita/0/11");

                if (response.IsSuccessStatusCode)
                {
                    content = await response.Content.ReadFromJsonAsync<List<Ricetta>>();
                }
            }
            catch (Exception e)
            { 
            }

            foreach (var item in content)
                {
                    //var response = await httpClient.GetAsync(apiUrl);
                    //Richiesta per foto
                    //Creazione oggetto
                    RicettaFoto elemento = new RicettaFoto(item, "TESTO");
                    ListaNovità.Add(elemento);
                }


        }
    }
}
