using DüsseldorferSchülerinventar.Models;
using DüsseldorferSchülerinventar.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace DüsseldorferSchülerinventar.ViewModels
{
    public partial class AdminViewModel : ObservableObject
    {
        private readonly AdminService _adminService;
        
        [ObservableProperty]
        private ObservableCollection<NormTable> _normTables;

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _statusMessage;

        public AdminViewModel(AdminService adminService)
        {
            _adminService = adminService;
            NormTables = new ObservableCollection<NormTable>();
        }

        [RelayCommand]
        private async Task LoadNormTables()
        {
            if (IsBusy) return;
            
            IsBusy = true;
            StatusMessage = "Lade Normtabellen...";
            
            try
            {
                NormTables.Clear();
                var tables = await _adminService.GetNormTablesAsync();
                
                foreach (var table in tables)
                {
                    NormTables.Add(table);
                }
                
                StatusMessage = $"{tables.Count} Normtabellen geladen";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Fehler: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task SaveNormTables()
        {
            if (IsBusy || NormTables.Count == 0) return;
            
            IsBusy = true;
            StatusMessage = "Speichere Änderungen...";
            
            try
            {
                var success = await _adminService.UpdateNormTablesAsync(NormTables);
                StatusMessage = success 
                    ? "Normtabellen erfolgreich aktualisiert" 
                    : "Speichern fehlgeschlagen";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Fehler: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}