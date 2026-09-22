using MudBlazor;

namespace GestionPedidos.Theme;

public static class AppTheme
{
    private static readonly string[] FontFamily = ["IBM Plex Sans", "sans-serif"];

    public static MudTheme Default => new()
    {
        PaletteLight = new PaletteLight
        {
            Primary = "#0C5CAB",
            Secondary = "#0A4A8A",
            Success = "#10B981",
            Warning = "#F59E0B",
            Error = "#EF4444",
            Background = "#FAFAFA",
            Surface = "#FFFFFF",
            AppbarBackground = "#FFFFFF",
            AppbarText = "#0A0A0A",
            DrawerBackground = "#FFFFFF",
            DrawerText = "#0A0A0A",
        },
        PaletteDark = new PaletteDark
        {
            Primary = "#0C5CAB",
            Secondary = "#3B82C4",
            Success = "#10B981",
            Warning = "#F59E0B",
            Error = "#EF4444",
            Background = "#09090B",
            Surface = "#141417",
            AppbarBackground = "#141417",
            DrawerBackground = "#141417",
            TextPrimary = "#FAFAFA",
        },
        LayoutProperties = new LayoutProperties
        {
            DefaultBorderRadius = "10px",
        },
        Typography = new Typography
        {
            Default = new DefaultTypography { FontFamily = FontFamily, FontSize = "0.875rem" },
            H1 = new H1Typography { FontFamily = FontFamily },
            H2 = new H2Typography { FontFamily = FontFamily },
            H3 = new H3Typography { FontFamily = FontFamily },
            H4 = new H4Typography { FontFamily = FontFamily, FontSize = "2rem" },
            H5 = new H5Typography { FontFamily = FontFamily, FontSize = "1.5rem" },
            H6 = new H6Typography { FontFamily = FontFamily, FontSize = "1.25rem" },
            Body1 = new Body1Typography { FontFamily = FontFamily, FontSize = "1rem" },
            Body2 = new Body2Typography { FontFamily = FontFamily, FontSize = "0.875rem" },
            Button = new ButtonTypography { FontFamily = FontFamily },
            Caption = new CaptionTypography { FontFamily = FontFamily, FontSize = "0.75rem" },
        },
    };
}
