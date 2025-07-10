using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tsc.GestaoDocumentos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoInicialBancoDados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NomeOrganizacao = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DataExpiracao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioCriacao = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioUltimaAlteracao = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TiposDocumento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PermiteMultiplosDocumentos = table.Column<bool>(type: "bit", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOrganizacao = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposDocumento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TiposDocumento_Tenants_IdOrganizacao",
                        column: x => x.IdOrganizacao,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TiposDono",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOrganizacao = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposDono", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TiposDono_Tenants_IdOrganizacao",
                        column: x => x.IdOrganizacao,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Login = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SenhaHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Perfil = table.Column<int>(type: "int", nullable: false),
                    UltimoAcesso = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOrganizacao = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Tenants_IdOrganizacao",
                        column: x => x.IdOrganizacao,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Documentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NomeArquivo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ChaveArmazenamento = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DataUpload = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TamanhoArquivo = table.Column<long>(type: "bigint", nullable: false),
                    TipoArquivo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Versao = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IdTipoDocumento = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOrganizacao = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documentos_Tenants_IdOrganizacao",
                        column: x => x.IdOrganizacao,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Documentos_TiposDocumento_IdTipoDocumento",
                        column: x => x.IdTipoDocumento,
                        principalTable: "TiposDocumento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DonosDocumento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NomeAmigavel = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IdTipoDono = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOrganizacao = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonosDocumento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DonosDocumento_Tenants_IdOrganizacao",
                        column: x => x.IdOrganizacao,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DonosDocumento_TiposDono_IdTipoDono",
                        column: x => x.IdTipoDono,
                        principalTable: "TiposDono",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TipoDonoTipoDocumento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdTipoDono = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdTipoDocumento = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOrganizacao = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoDonoTipoDocumento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TipoDonoTipoDocumento_TiposDocumento_IdTipoDocumento",
                        column: x => x.IdTipoDocumento,
                        principalTable: "TiposDocumento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TipoDonoTipoDocumento_TiposDono_IdTipoDono",
                        column: x => x.IdTipoDono,
                        principalTable: "TiposDono",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LogsAuditoria",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdUsuario = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntidadeAfetada = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IdEntidade = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Operacao = table.Column<int>(type: "int", nullable: false),
                    DadosAnteriores = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DadosNovos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataHoraOperacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IpUsuario = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    UserAgent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOrganizacao = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogsAuditoria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogsAuditoria_Tenants_IdOrganizacao",
                        column: x => x.IdOrganizacao,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LogsAuditoria_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DocumentoDonoDocumento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdDocumento = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdDonoDocumento = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOrganizacao = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentoDonoDocumento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentoDonoDocumento_Documentos_IdDocumento",
                        column: x => x.IdDocumento,
                        principalTable: "Documentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentoDonoDocumento_DonosDocumento_IdDonoDocumento",
                        column: x => x.IdDonoDocumento,
                        principalTable: "DonosDocumento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentoDonoDocumento_IdDocumento",
                table: "DocumentoDonoDocumento",
                column: "IdDocumento");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentoDonoDocumento_IdDonoDocumento",
                table: "DocumentoDonoDocumento",
                column: "IdDonoDocumento");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentoDonoDocumento_IdOrganizacao_IdDocumento_IdDonoDocumento",
                table: "DocumentoDonoDocumento",
                columns: new[] { "IdOrganizacao", "IdDocumento", "IdDonoDocumento" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_IdOrganizacao_ChaveArmazenamento",
                table: "Documentos",
                columns: new[] { "IdOrganizacao", "ChaveArmazenamento" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_IdOrganizacao_DataUpload",
                table: "Documentos",
                columns: new[] { "IdOrganizacao", "DataUpload" });

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_IdOrganizacao_IdTipoDocumento",
                table: "Documentos",
                columns: new[] { "IdOrganizacao", "IdTipoDocumento" });

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_IdOrganizacao_Status",
                table: "Documentos",
                columns: new[] { "IdOrganizacao", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_IdTipoDocumento",
                table: "Documentos",
                column: "IdTipoDocumento");

            migrationBuilder.CreateIndex(
                name: "IX_DonosDocumento_IdOrganizacao_IdTipoDono",
                table: "DonosDocumento",
                columns: new[] { "IdOrganizacao", "IdTipoDono" });

            migrationBuilder.CreateIndex(
                name: "IX_DonosDocumento_IdOrganizacao_NomeAmigavel",
                table: "DonosDocumento",
                columns: new[] { "IdOrganizacao", "NomeAmigavel" });

            migrationBuilder.CreateIndex(
                name: "IX_DonosDocumento_IdTipoDono",
                table: "DonosDocumento",
                column: "IdTipoDono");

            migrationBuilder.CreateIndex(
                name: "IX_LogsAuditoria_IdOrganizacao_DataHoraOperacao",
                table: "LogsAuditoria",
                columns: new[] { "IdOrganizacao", "DataHoraOperacao" });

            migrationBuilder.CreateIndex(
                name: "IX_LogsAuditoria_IdOrganizacao_EntidadeAfetada_EntidadeId",
                table: "LogsAuditoria",
                columns: new[] { "IdOrganizacao", "EntidadeAfetada", "IdEntidade" });

            migrationBuilder.CreateIndex(
                name: "IX_LogsAuditoria_IdOrganizacao_IdUsuario",
                table: "LogsAuditoria",
                columns: new[] { "IdOrganizacao", "IdUsuario" });

            migrationBuilder.CreateIndex(
                name: "IX_LogsAuditoria_IdOrganizacao_Operacao",
                table: "LogsAuditoria",
                columns: new[] { "IdOrganizacao", "Operacao" });

            migrationBuilder.CreateIndex(
                name: "IX_LogsAuditoria_IdUsuario",
                table: "LogsAuditoria",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_NomeOrganizacao",
                table: "Tenants",
                column: "NomeOrganizacao");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_Slug",
                table: "Tenants",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_Status",
                table: "Tenants",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TipoDonoTipoDocumento_IdOrganizacao_IdTipoDono_IdTipoDocumento",
                table: "TipoDonoTipoDocumento",
                columns: new[] { "IdOrganizacao", "IdTipoDono", "IdTipoDocumento" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TipoDonoTipoDocumento_IdTipoDocumento",
                table: "TipoDonoTipoDocumento",
                column: "IdTipoDocumento");

            migrationBuilder.CreateIndex(
                name: "IX_TipoDonoTipoDocumento_IdTipoDono",
                table: "TipoDonoTipoDocumento",
                column: "IdTipoDono");

            migrationBuilder.CreateIndex(
                name: "IX_TiposDocumento_IdOrganizacao_Nome",
                table: "TiposDocumento",
                columns: new[] { "IdOrganizacao", "Nome" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TiposDono_IdOrganizacao_Nome",
                table: "TiposDono",
                columns: new[] { "IdOrganizacao", "Nome" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdOrganizacao_Email",
                table: "Usuarios",
                columns: new[] { "IdOrganizacao", "Email" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdOrganizacao_Login",
                table: "Usuarios",
                columns: new[] { "IdOrganizacao", "Login" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdOrganizacao_Perfil",
                table: "Usuarios",
                columns: new[] { "IdOrganizacao", "Perfil" });

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdOrganizacao_Status",
                table: "Usuarios",
                columns: new[] { "IdOrganizacao", "Status" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentoDonoDocumento");

            migrationBuilder.DropTable(
                name: "LogsAuditoria");

            migrationBuilder.DropTable(
                name: "TipoDonoTipoDocumento");

            migrationBuilder.DropTable(
                name: "Documentos");

            migrationBuilder.DropTable(
                name: "DonosDocumento");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "TiposDocumento");

            migrationBuilder.DropTable(
                name: "TiposDono");

            migrationBuilder.DropTable(
                name: "Tenants");
        }
    }
}
