using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BillingService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoOpcaoDePagamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "OpcaoPagamentoSelecionada_ComJuros",
                table: "PropostasAprovadaEvento",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OpcaoPagamentoSelecionada_QuantidadeParcelas",
                table: "PropostasAprovadaEvento",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OpcaoPagamentoSelecionada_ValorParcela",
                table: "PropostasAprovadaEvento",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OpcaoPagamentoSelecionada_ValorTotalComJuros",
                table: "PropostasAprovadaEvento",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OpcaoPagamentoSelecionada_ComJuros",
                table: "PropostasAprovadaEvento");

            migrationBuilder.DropColumn(
                name: "OpcaoPagamentoSelecionada_QuantidadeParcelas",
                table: "PropostasAprovadaEvento");

            migrationBuilder.DropColumn(
                name: "OpcaoPagamentoSelecionada_ValorParcela",
                table: "PropostasAprovadaEvento");

            migrationBuilder.DropColumn(
                name: "OpcaoPagamentoSelecionada_ValorTotalComJuros",
                table: "PropostasAprovadaEvento");
        }
    }
}
