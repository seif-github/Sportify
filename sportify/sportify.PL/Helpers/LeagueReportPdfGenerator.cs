using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Drawing;
using QuestPDF.Elements;
using sportify.BLL.DTOs;
using System;
using System.Globalization;
using System.Composition;
using sportify.DAL.Entities;
using static QuestPDF.Helpers.Colors;

public static class LeagueReportPdfGenerator
{
    public static byte[] GenerateLeagueReportPdf(LeagueReportDTO report)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(40);
                page.Size(PageSizes.A4);
                page.DefaultTextStyle(x => x.FontSize(12));
                page.Header().Element(c => ComposeHeader(c, report));
                page.Content().Element(c => ComposeContent(c, report));
                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Sportify © 2025 - Page ");
                    x.CurrentPageNumber();
                });
            });
        }).GeneratePdf();
    }

    private static void ComposeHeader(IContainer container, LeagueReportDTO report)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(col =>
            {
                col.Item().Text("Sportify League Report").Bold().FontSize(18).FontColor(Colors.Blue.Darken2);
                col.Item().Text("Professional League Summary").FontSize(10).FontColor(Colors.Grey.Darken2);
                col.Item().Text($"Generated on: {report.GeneratedAt:dd MMM yyyy hh:mm tt}").FontSize(10).FontColor(Colors.Grey.Darken2);
            });

            row.ConstantItem(80).Image("wwwroot/assets/logo.png");
        });
    }

    private static void ComposeContent(IContainer container, LeagueReportDTO report)
    {
        container.PaddingVertical(10).Column(col =>
        {
            //string defaultLogoPath = "wwwroot/assets/default-league-logo.png";
            //string logoPath = $"wwwroot/images/{report.ImageUrl}";
            //if (!string.IsNullOrEmpty(report.ImageUrl))
            //{
            //    if (File.Exists(logoPath))
            //    {
            //        col.Item()
            //            .Width(50)
            //            .Height(50)
            //            .AlignMiddle()
            //            .AlignCenter()
            //            .Image(logoPath);
            //    }
            //    else
            //    {
            //        col.Item()
            //            .Width(50)
            //            .Height(50)
            //            .AlignMiddle()
            //            .AlignCenter()
            //            .Image(defaultLogoPath);
            //    }
            //}
            //else
            //{
            //    col.Item()
            //        .Width(50)
            //        .Height(50)
            //        .AlignMiddle()
            //        .AlignCenter()
            //        .Image(defaultLogoPath);
            //}
            col.Item().Text($"League: {report.LeagueName}").Bold().FontSize(14);
            col.Item().Text($"Organizer: {report.OrganizerName}");
            col.Item().Text($"Start Date: {report.StartDate:dd MMM yyyy}");
            col.Item().Text($"End Date: {report.StartDate.AddDays((report.NumberOfTeams - 1) * report.DurationBetweenMatches):dd MMM yyyy}");
            col.Item().Text($"Number of Teams: {report.NumberOfTeams}");
            col.Item().Text($"Duration Between Rounds: {report.DurationBetweenMatches} days");
            col.Item().Text($"Round Robin: {(report.RoundRobin ? "Yes (Home & Away)" : "No (Single Round)")}");
            col.Item().Text($"Total Matches Played: {report.TotalMatchesPlayed}");

            col.Item().PaddingTop(10).Element(ComposeHighlights(report));
            col.Item().PaddingTop(10).Element(ComposeStandingsTable(report));
            col.Item().PaddingTop(10).Element(ComposeMatchesTable(report));
        });
    }

    private static Action<IContainer> ComposeStandingsTable(LeagueReportDTO report) => container =>
    {
        container.Column(col =>
        {
            col.Item().Text("Standings").Bold().FontSize(14).FontColor(Colors.Blue.Medium);

            col.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(40); // #
                    //columns.ConstantColumn(40); // Logo
                    columns.RelativeColumn(); // Name
                    columns.ConstantColumn(40); // W
                    columns.ConstantColumn(40); // D
                    columns.ConstantColumn(40); // L
                    columns.ConstantColumn(50); // GS
                    columns.ConstantColumn(50); // GC
                    columns.ConstantColumn(50); // Pts
                });

                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("#").Bold();
                    //header.Cell().Element(CellStyle).Text("Logo").Bold();
                    header.Cell().Element(CellStyle).Text("Team").Bold();
                    header.Cell().Element(CellStyle).Text("W").Bold();
                    header.Cell().Element(CellStyle).Text("D").Bold();
                    header.Cell().Element(CellStyle).Text("L").Bold();
                    header.Cell().Element(CellStyle).Text("GS").Bold();
                    header.Cell().Element(CellStyle).Text("GC").Bold();
                    header.Cell().Element(CellStyle).Text("Pts").Bold();

                    static IContainer CellStyle(IContainer container) =>
                        container.DefaultTextStyle(x => x.SemiBold()).Border(1).BorderColor(Colors.Black).Padding(5).Background(Colors.Grey.Lighten3);
                });

                int i = 1;
                foreach (var team in report.Teams)
                {
                    table.Cell().Element(CellStyle).AlignCenter().Text(i.ToString()); i++;
                    //string defaultLogoPath = "wwwroot/assets/default-team-logo.png";
                    //string logoPath = $"wwwroot/images/{team.ImageUrl}";
                    //if (!string.IsNullOrEmpty(team.ImageUrl))
                    //{
                    //    if (File.Exists(logoPath))
                    //    {
                    //        table.Cell()
                    //            .Width(20)
                    //            .Height(20)
                    //            .AlignMiddle()
                    //            .AlignCenter()
                    //            .Image(logoPath);
                    //    }
                    //    else
                    //    {
                    //        table.Cell()
                    //            .Width(20)
                    //            .Height(20)
                    //            .AlignMiddle()
                    //            .AlignCenter()
                    //            .Image(defaultLogoPath);
                    //    }
                    //}
                    //else
                    //{
                    //    table.Cell()
                    //        .Width(20)
                    //        .Height(20)
                    //        .AlignMiddle()
                    //        .AlignCenter()
                    //        .Image(defaultLogoPath);
                    //}
                    table.Cell().Element(CellStyle).Text(team.Name);
                    table.Cell().Element(CellStyle).AlignCenter().Text(team.Wins.ToString());
                    table.Cell().Element(CellStyle).AlignCenter().Text(team.Draws.ToString());
                    table.Cell().Element(CellStyle).AlignCenter().Text(team.Losses.ToString());
                    table.Cell().Element(CellStyle).AlignCenter().Text(team.GoalsScored.ToString());
                    table.Cell().Element(CellStyle).AlignCenter().Text(team.GoalsConceded.ToString());
                    table.Cell().Element(CellStyle).AlignCenter().Text(team.Points.ToString());

                    static IContainer CellStyle(IContainer container) =>
                        container.Border(1).BorderColor(Colors.Black).PaddingVertical(5).PaddingHorizontal(2);
                }
            });
        });
    };

    private static Action<IContainer> ComposeMatchesTable(LeagueReportDTO report) => container =>
    {
        container.PaddingTop(20).Column(col =>
        {
            col.Item().Text("All Matches").Bold().FontSize(14).FontColor(Colors.Blue.Medium);

            col.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(150); // Date
                    columns.RelativeColumn(); // Home Team
                    columns.ConstantColumn(40); // vs
                    columns.RelativeColumn(); // Away Team
                    columns.ConstantColumn(40); // Score
                    columns.ConstantColumn(80); // Status
                });

                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("Date").Bold();
                    header.Cell().Element(CellStyle).Text("Home Team").Bold();
                    header.Cell().Element(CellStyle);
                    header.Cell().Element(CellStyle).Text("Away Team").Bold();
                    header.Cell().Element(CellStyle).Text("Score").Bold();
                    header.Cell().Element(CellStyle).Text("Status").Bold();

                    static IContainer CellStyle(IContainer container) =>
                        container.DefaultTextStyle(x => x.SemiBold()).Border(1).BorderColor(Colors.Black).Padding(5).Background(Colors.Grey.Lighten3);
                });

                foreach (var match in report.Matches)
                {
                    table.Cell().Element(CellStyle).Text(match.Date.ToString("dd MMM yyyy h mm tt"));
                    table.Cell().Element(CellStyle).Text(match.FirstTeamName);
                    table.Cell().Element(CellStyle).AlignCenter().Text("vs");
                    table.Cell().Element(CellStyle).Text(match.SecondTeamName);

                    if (match.IsCompleted)
                    {
                        table.Cell().Element(CellStyle).AlignCenter()
                            .Text($"{match.FirstTeamGoals} - {match.SecondTeamGoals}");
                    }
                    else
                    {
                        table.Cell().Element(CellStyle).AlignCenter().Text("-");
                    }

                    table.Cell().Element(CellStyle).Text(match.IsCompleted ? "Completed" : "Pending");

                    static IContainer CellStyle(IContainer container) =>
                        container.Border(1).BorderColor(Colors.Black).PaddingVertical(3).PaddingHorizontal(5);
                }
            });
        });
    };

    private static Action<IContainer> ComposeHighlights(LeagueReportDTO report) => container =>
    {
        container.Column(col =>
        {
            col.Item().Text("Highlights").Bold().FontSize(14).FontColor(Colors.Blue.Medium);
            col.Item().Text($"Top Scoring Team: {report.TopScoringTeam}");
            col.Item().Text($"Most Goals Conceded Team: {report.MostGoalsConcededTeam}");
            col.Item().Text($"Highest Scoring Match: {report.TopScoringMatch.Team1} {report.TopScoringMatch.Goals1} - {report.TopScoringMatch.Goals2} {report.TopScoringMatch.Team2}");
        });
    };
}
