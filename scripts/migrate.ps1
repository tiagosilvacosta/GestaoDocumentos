# Script PowerShell para operações de migração do Entity Framework
# Sistema de Gestão de Documentos

param(
    [Parameter(Mandatory=$true)]
    [ValidateSet("add", "update", "remove", "list", "script", "drop")]
    [string]$Action,
    
    [string]$MigrationName = "",
    [string]$TargetMigration = "",
    [switch]$Force
)

# Configurações
$InfraProject = "src\Tsc.GestaoDocumentos.Infrastructure"
$StartupProject = "src\Tsc.GestaoDocumentos.Api"
$OutputDir = "scripts\migrations"

# Cores para output
$SuccessColor = "Green"
$ErrorColor = "Red"
$InfoColor = "Yellow"

function Write-Success($message) {
    Write-Host $message -ForegroundColor $SuccessColor
}

function Write-Error($message) {
    Write-Host $message -ForegroundColor $ErrorColor
}

function Write-Info($message) {
    Write-Host $message -ForegroundColor $InfoColor
}

function Test-Prerequisites {
    Write-Info "Verificando pré-requisitos..."
    
    # Verificar se dotnet ef está instalado
    try {
        $efVersion = dotnet ef --version 2>$null
        if ($LASTEXITCODE -eq 0) {
            Write-Success "? Entity Framework Core Tools encontrado: $efVersion"
        } else {
            throw "EF Tools não encontrado"
        }
    } catch {
        Write-Error "? Entity Framework Core Tools não está instalado"
        Write-Info "Execute: dotnet tool install --global dotnet-ef"
        exit 1
    }
    
    # Verificar se os projetos existem
    if (!(Test-Path $InfraProject)) {
        Write-Error "? Projeto Infrastructure não encontrado: $InfraProject"
        exit 1
    }
    
    if (!(Test-Path $StartupProject)) {
        Write-Error "? Projeto API não encontrado: $StartupProject"
        exit 1
    }
    
    Write-Success "? Pré-requisitos verificados com sucesso"
}

function Add-Migration {
    param([string]$Name)
    
    if ([string]::IsNullOrWhiteSpace($Name)) {
        Write-Error "Nome da migração é obrigatório para a ação 'add'"
        Write-Info "Uso: .\migrate.ps1 -Action add -MigrationName 'NomeDaMigracao'"
        exit 1
    }
    
    Write-Info "Criando migração: $Name"
    
    $command = "dotnet ef migrations add `"$Name`" --project `"$InfraProject`" --startup-project `"$StartupProject`""
    Write-Info "Executando: $command"
    
    Invoke-Expression $command
    
    if ($LASTEXITCODE -eq 0) {
        Write-Success "? Migração '$Name' criada com sucesso"
    } else {
        Write-Error "? Erro ao criar migração"
        exit 1
    }
}

function Update-Database {
    param([string]$Target = "")
    
    Write-Info "Aplicando migrações ao banco de dados..."
    
    $command = "dotnet ef database update"
    if (![string]::IsNullOrWhiteSpace($Target)) {
        $command += " `"$Target`""
        Write-Info "Migrando para: $Target"
    }
    $command += " --project `"$InfraProject`" --startup-project `"$StartupProject`""
    
    Write-Info "Executando: $command"
    
    Invoke-Expression $command
    
    if ($LASTEXITCODE -eq 0) {
        Write-Success "? Banco de dados atualizado com sucesso"
    } else {
        Write-Error "? Erro ao atualizar banco de dados"
        exit 1
    }
}

function Remove-Migration {
    if (!$Force) {
        $confirmation = Read-Host "Tem certeza que deseja remover a última migração? (s/N)"
        if ($confirmation -ne 's' -and $confirmation -ne 'S') {
            Write-Info "Operação cancelada"
            return
        }
    }
    
    Write-Info "Removendo última migração..."
    
    $command = "dotnet ef migrations remove --project `"$InfraProject`" --startup-project `"$StartupProject`""
    Write-Info "Executando: $command"
    
    Invoke-Expression $command
    
    if ($LASTEXITCODE -eq 0) {
        Write-Success "? Migração removida com sucesso"
    } else {
        Write-Error "? Erro ao remover migração"
        exit 1
    }
}

function List-Migrations {
    Write-Info "Listando migrações..."
    
    $command = "dotnet ef migrations list --project `"$InfraProject`" --startup-project `"$StartupProject`""
    Write-Info "Executando: $command"
    
    Invoke-Expression $command
    
    if ($LASTEXITCODE -eq 0) {
        Write-Success "? Lista de migrações exibida"
    } else {
        Write-Error "? Erro ao listar migrações"
        exit 1
    }
}

function Generate-Script {
    param([string]$From = "", [string]$To = "")
    
    # Criar diretório de output se não existir
    if (!(Test-Path $OutputDir)) {
        New-Item -ItemType Directory -Path $OutputDir -Force | Out-Null
        Write-Success "? Diretório criado: $OutputDir"
    }
    
    $timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
    $scriptFile = "$OutputDir\migration_script_$timestamp.sql"
    
    Write-Info "Gerando script SQL para migração..."
    
    $command = "dotnet ef migrations script"
    if (![string]::IsNullOrWhiteSpace($From)) {
        $command += " `"$From`""
    }
    if (![string]::IsNullOrWhiteSpace($To)) {
        $command += " `"$To`""
    }
    $command += " --project `"$InfraProject`" --startup-project `"$StartupProject`" --output `"$scriptFile`""
    
    Write-Info "Executando: $command"
    
    Invoke-Expression $command
    
    if ($LASTEXITCODE -eq 0) {
        Write-Success "? Script SQL gerado: $scriptFile"
    } else {
        Write-Error "? Erro ao gerar script SQL"
        exit 1
    }
}

function Drop-Database {
    if (!$Force) {
        Write-Error "ATENÇÃO: Esta operação irá DELETAR COMPLETAMENTE o banco de dados!"
        $confirmation = Read-Host "Tem ABSOLUTA certeza que deseja continuar? Digite 'DELETE' para confirmar"
        if ($confirmation -ne "DELETE") {
            Write-Info "Operação cancelada"
            return
        }
    }
    
    Write-Info "Removendo banco de dados..."
    
    $command = "dotnet ef database drop --project `"$InfraProject`" --startup-project `"$StartupProject`" --force"
    Write-Info "Executando: $command"
    
    Invoke-Expression $command
    
    if ($LASTEXITCODE -eq 0) {
        Write-Success "? Banco de dados removido"
    } else {
        Write-Error "? Erro ao remover banco de dados"
        exit 1
    }
}

# Função principal
function Main {
    Test-Prerequisites
    
    switch ($Action.ToLower()) {
        "add" { 
            Add-Migration -Name $MigrationName 
        }
        "update" { 
            Update-Database -Target $TargetMigration 
        }
        "remove" { 
            Remove-Migration 
        }
        "list" { 
            List-Migrations 
        }
        "script" { 
            Generate-Script -From $MigrationName -To $TargetMigration 
        }
        "drop" { 
            Drop-Database 
        }
        default {
            Write-Error "Ação inválida: $Action"
            Write-Info @"
Uso: .\migrate.ps1 -Action <ação> [parâmetros]

Ações disponíveis:
  add     - Criar nova migração (-MigrationName obrigatório)
  update  - Aplicar migrações (-TargetMigration opcional)
  remove  - Remover última migração (-Force para pular confirmação)
  list    - Listar todas as migrações
  script  - Gerar script SQL (-MigrationName e -TargetMigration opcionais)
  drop    - Remover banco de dados (-Force para pular confirmação)

Exemplos:
  .\migrate.ps1 -Action add -MigrationName "AdicionarTabelaUsuarios"
  .\migrate.ps1 -Action update
  .\migrate.ps1 -Action remove -Force
  .\migrate.ps1 -Action script -MigrationName "20240101_Inicial" -TargetMigration "20240201_Final"
"@
            exit 1
        }
    }
}

# Executar
Main