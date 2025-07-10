# Script PowerShell para opera��es de migra��o do Entity Framework
# Sistema de Gest�o de Documentos

param(
    [Parameter(Mandatory=$true)]
    [ValidateSet("add", "update", "remove", "list", "script", "drop")]
    [string]$Action,
    
    [string]$MigrationName = "",
    [string]$TargetMigration = "",
    [switch]$Force
)

# Configura��es
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
    Write-Info "Verificando pr�-requisitos..."
    
    # Verificar se dotnet ef est� instalado
    try {
        $efVersion = dotnet ef --version 2>$null
        if ($LASTEXITCODE -eq 0) {
            Write-Success "? Entity Framework Core Tools encontrado: $efVersion"
        } else {
            throw "EF Tools n�o encontrado"
        }
    } catch {
        Write-Error "? Entity Framework Core Tools n�o est� instalado"
        Write-Info "Execute: dotnet tool install --global dotnet-ef"
        exit 1
    }
    
    # Verificar se os projetos existem
    if (!(Test-Path $InfraProject)) {
        Write-Error "? Projeto Infrastructure n�o encontrado: $InfraProject"
        exit 1
    }
    
    if (!(Test-Path $StartupProject)) {
        Write-Error "? Projeto API n�o encontrado: $StartupProject"
        exit 1
    }
    
    Write-Success "? Pr�-requisitos verificados com sucesso"
}

function Add-Migration {
    param([string]$Name)
    
    if ([string]::IsNullOrWhiteSpace($Name)) {
        Write-Error "Nome da migra��o � obrigat�rio para a a��o 'add'"
        Write-Info "Uso: .\migrate.ps1 -Action add -MigrationName 'NomeDaMigracao'"
        exit 1
    }
    
    Write-Info "Criando migra��o: $Name"
    
    $command = "dotnet ef migrations add `"$Name`" --project `"$InfraProject`" --startup-project `"$StartupProject`""
    Write-Info "Executando: $command"
    
    Invoke-Expression $command
    
    if ($LASTEXITCODE -eq 0) {
        Write-Success "? Migra��o '$Name' criada com sucesso"
    } else {
        Write-Error "? Erro ao criar migra��o"
        exit 1
    }
}

function Update-Database {
    param([string]$Target = "")
    
    Write-Info "Aplicando migra��es ao banco de dados..."
    
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
        $confirmation = Read-Host "Tem certeza que deseja remover a �ltima migra��o? (s/N)"
        if ($confirmation -ne 's' -and $confirmation -ne 'S') {
            Write-Info "Opera��o cancelada"
            return
        }
    }
    
    Write-Info "Removendo �ltima migra��o..."
    
    $command = "dotnet ef migrations remove --project `"$InfraProject`" --startup-project `"$StartupProject`""
    Write-Info "Executando: $command"
    
    Invoke-Expression $command
    
    if ($LASTEXITCODE -eq 0) {
        Write-Success "? Migra��o removida com sucesso"
    } else {
        Write-Error "? Erro ao remover migra��o"
        exit 1
    }
}

function List-Migrations {
    Write-Info "Listando migra��es..."
    
    $command = "dotnet ef migrations list --project `"$InfraProject`" --startup-project `"$StartupProject`""
    Write-Info "Executando: $command"
    
    Invoke-Expression $command
    
    if ($LASTEXITCODE -eq 0) {
        Write-Success "? Lista de migra��es exibida"
    } else {
        Write-Error "? Erro ao listar migra��es"
        exit 1
    }
}

function Generate-Script {
    param([string]$From = "", [string]$To = "")
    
    # Criar diret�rio de output se n�o existir
    if (!(Test-Path $OutputDir)) {
        New-Item -ItemType Directory -Path $OutputDir -Force | Out-Null
        Write-Success "? Diret�rio criado: $OutputDir"
    }
    
    $timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
    $scriptFile = "$OutputDir\migration_script_$timestamp.sql"
    
    Write-Info "Gerando script SQL para migra��o..."
    
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
        Write-Error "ATEN��O: Esta opera��o ir� DELETAR COMPLETAMENTE o banco de dados!"
        $confirmation = Read-Host "Tem ABSOLUTA certeza que deseja continuar? Digite 'DELETE' para confirmar"
        if ($confirmation -ne "DELETE") {
            Write-Info "Opera��o cancelada"
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

# Fun��o principal
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
            Write-Error "A��o inv�lida: $Action"
            Write-Info @"
Uso: .\migrate.ps1 -Action <a��o> [par�metros]

A��es dispon�veis:
  add     - Criar nova migra��o (-MigrationName obrigat�rio)
  update  - Aplicar migra��es (-TargetMigration opcional)
  remove  - Remover �ltima migra��o (-Force para pular confirma��o)
  list    - Listar todas as migra��es
  script  - Gerar script SQL (-MigrationName e -TargetMigration opcionais)
  drop    - Remover banco de dados (-Force para pular confirma��o)

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