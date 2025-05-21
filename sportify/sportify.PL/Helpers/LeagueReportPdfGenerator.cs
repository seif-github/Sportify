using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using sportify.BLL.DTOs;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

namespace sportify.PL.Helpers
{
    public class LeagueReportPdfGenerator
    {
        public byte[] Generate(LeagueReportDTO report)
        {
            var document = new PdfDocument();
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            var font = new XFont("Arial", 12);
            int y = 40;

            void DrawLine(string text, int fontSize = 12, bool bold = false)
            {
                var style = bold ? XFontStyle.Bold : XFontStyle.Regular;
                var xFont = new XFont("Arial", fontSize, style);
                gfx.DrawString(text, xFont, XBrushes.Black, new XRect(40, y, page.Width - 80, page.Height), XStringFormats.TopLeft);
                y += 25;
            }

            // Title
            DrawLine($"League Report - {report.LeagueName}", 16, bold: true);
            y += 10;

            // Basic Info
            DrawLine($"Organizer ID: {report.OrganizerId}");
            DrawLine($"Start Date: {report.StartDate.ToShortDateString()}");
            DrawLine($"Number of Teams: {report.NumberOfTeams}");
            DrawLine($"Duration Between Matches (Days): {report.DurationBetweenMatches}");
            DrawLine($"Round Robin: {(report.RoundRobin ? "Yes" : "No")}");
            DrawLine($"Total Matches Played: {report.TotalMatchesPlayed}");
            DrawLine($"Top Scoring Team: {report.TopScoringTeam}");
            DrawLine($"Most Goals Conceded Team: {report.MostGoalsConcededTeam}");

            y += 15;
            DrawLine("Teams Summary:", 14, bold: true);
            y += 5;

            foreach (var team in report.Teams)
            {
                DrawLine($"Team: {team.Name}", bold: true);
                DrawLine($"  Wins: {team.Wins}, Draws: {team.Draws}, Losses: {team.Losses}");
                DrawLine($"  Goals Scored: {team.GoalsScored}, Goals Conceded: {team.GoalsConceded}");
                DrawLine($"  Points: {team.Points}");
                y += 10;
            }

            using (var stream = new MemoryStream())
            {
                document.Save(stream, false);
                return stream.ToArray();
            }
        }
    }
}
