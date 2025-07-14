using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace qr_website.Services
{
    public class ThemeService
    {
        public bool IsDarkMode { get; private set; }
        public event Action? OnThemeChanged;
        private readonly IJSRuntime _jsRuntime;

        public ThemeService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task InitializeAsync()
        {
            var theme = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "theme");
            IsDarkMode = theme == "dark";
            await SetThemeAsync(IsDarkMode);
        }

        public async Task ToggleTheme()
        {
            IsDarkMode = !IsDarkMode;
            await SetThemeAsync(IsDarkMode);
            OnThemeChanged?.Invoke();
        }

        private async Task SetThemeAsync(bool isDark)
        {
            await _jsRuntime.InvokeVoidAsync("applyTheme", isDark ? "dark" : "light");
        }
    }
}