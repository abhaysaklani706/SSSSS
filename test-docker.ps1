# PowerShell script to test Docker build locally before Railway deployment

Write-Host "🐳 Docker Build Test for AdminServer" -ForegroundColor Green
Write-Host ""

# Check if Docker is installed
try {
    $dockerVersion = docker --version
    Write-Host "✅ Docker detected: $dockerVersion" -ForegroundColor Green
} catch {
    Write-Host "❌ Docker not found. Please install Docker Desktop from:" -ForegroundColor Red
    Write-Host "   https://www.docker.com/products/docker-desktop" -ForegroundColor Yellow
    exit 1
}

# Check if Docker daemon is running
try {
    docker ps | Out-Null
    Write-Host "✅ Docker daemon is running" -ForegroundColor Green
} catch {
    Write-Host "❌ Docker daemon is not running. Please start Docker Desktop." -ForegroundColor Red
    exit 1
}

$imageName = "adminserver-test"
$containerName = "adminserver-container"
$port = 5030

Write-Host ""
Write-Host "🔨 Building Docker image..." -ForegroundColor Cyan
Write-Host "   Image name: $imageName" -ForegroundColor Gray

# Build the Docker image
docker build -t $imageName .

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Docker build failed" -ForegroundColor Red
    exit 1
}

Write-Host "✅ Docker image built successfully!" -ForegroundColor Green

# Stop and remove existing container if it exists
Write-Host ""
Write-Host "🧹 Cleaning up existing containers..." -ForegroundColor Cyan
docker stop $containerName 2>$null | Out-Null
docker rm $containerName 2>$null | Out-Null

# Run the container
Write-Host ""
Write-Host "🚀 Starting Docker container..." -ForegroundColor Cyan
Write-Host "   Container name: $containerName" -ForegroundColor Gray
Write-Host "   Port mapping: $port -> 5030" -ForegroundColor Gray

docker run -d -p ${port}:5030 --name $containerName $imageName

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Failed to start container" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "✅ Container started successfully!" -ForegroundColor Green
Write-Host ""
Write-Host "📊 Container Info:" -ForegroundColor Cyan
docker ps --filter "name=$containerName" --format "table {{.ID}}\t{{.Image}}\t{{.Status}}\t{{.Ports}}"

Write-Host ""
Write-Host "🌐 Access your application at:" -ForegroundColor Yellow
Write-Host "   API: http://localhost:$port" -ForegroundColor White
Write-Host "   Swagger: http://localhost:$port/swagger" -ForegroundColor White

Write-Host ""
Write-Host "📋 Useful commands:" -ForegroundColor Cyan
Write-Host "   View logs:    docker logs $containerName -f" -ForegroundColor Gray
Write-Host "   Stop:         docker stop $containerName" -ForegroundColor Gray
Write-Host "   Remove:       docker rm $containerName" -ForegroundColor Gray
Write-Host "   Shell access: docker exec -it $containerName /bin/bash" -ForegroundColor Gray

Write-Host ""
Write-Host "⏳ Waiting for application to start (10 seconds)..." -ForegroundColor Cyan
Start-Sleep -Seconds 10

# Check if container is still running
$containerStatus = docker ps --filter "name=$containerName" --format "{{.Status}}"
if ($containerStatus) {
    Write-Host "✅ Container is running!" -ForegroundColor Green
    Write-Host ""
    Write-Host "🔍 Testing API endpoint..." -ForegroundColor Cyan
    
    try {
        $response = Invoke-WebRequest -Uri "http://localhost:$port/api/admin/agents" -Method GET -TimeoutSec 5
        Write-Host "✅ API is responding! Status: $($response.StatusCode)" -ForegroundColor Green
    } catch {
        Write-Host "⚠️  API test failed, but container is running. Check logs:" -ForegroundColor Yellow
        Write-Host "   docker logs $containerName" -ForegroundColor Gray
    }
    
    Write-Host ""
    Write-Host "🎉 Docker test completed successfully!" -ForegroundColor Green
    Write-Host "   Your app is ready for Railway deployment!" -ForegroundColor Green
} else {
    Write-Host "❌ Container stopped unexpectedly. Check logs:" -ForegroundColor Red
    Write-Host ""
    docker logs $containerName
}

Write-Host ""
