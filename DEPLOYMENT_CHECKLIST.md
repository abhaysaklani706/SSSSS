# 🚀 Railway Deployment Checklist

Use this checklist to ensure your AdminServer is ready for Railway deployment.

## ✅ Pre-Deployment Checklist

### 📁 Files & Configuration

- [x] **Dockerfile** - Multi-stage .NET 8 Docker build
- [x] **.dockerignore** - Excludes unnecessary files from Docker build
- [x] **railway.json** - Railway platform configuration
- [x] **.gitignore** - Git ignore file for .NET projects
- [x] **AdminServer.sln** - Solution file for Visual Studio
- [x] **README.md** - Comprehensive project documentation
- [x] **RAILWAY_DEPLOYMENT.md** - Detailed deployment guide
- [x] **run-local.ps1** - Script to run locally
- [x] **test-docker.ps1** - Script to test Docker build

### 🔧 Code Configuration

- [x] **PORT Environment Variable** - Configured in `Program.cs` line 44
- [x] **CORS Enabled** - Allows cross-origin requests
- [x] **Swagger Enabled** - Available in all environments
- [x] **SignalR Hubs** - Configured at `/adminHub` and `/agentHub`
- [x] **All Dependencies** - Specified in `AdminServerStub.csproj`

### 🏗️ Build Verification

- [x] **Project Builds** - Verified with `dotnet build`
- [x] **No Compilation Errors** - Build succeeded
- [x] **Only Platform Warnings** - Windows-specific System.Management (OK)

## 📋 Before Railway Deployment

### Step 1: Test Locally (Optional but Recommended)

```powershell
# Test with .NET directly
.\run-local.ps1

# Or test with Docker
.\test-docker.ps1
```

- [ ] Local build succeeds
- [ ] Application starts without errors
- [ ] Swagger UI accessible at http://localhost:5030/swagger
- [ ] API endpoints respond correctly
- [ ] No runtime errors in console

### Step 2: Prepare Git Repository

```bash
# Initialize Git (if not done)
git init

# Add all files
git add .

# Commit
git commit -m "Ready for Railway deployment"

# Create GitHub repo and push
git remote add origin https://github.com/YOUR_USERNAME/AdminServer.git
git branch -M main
git push -u origin main
```

- [ ] Git repository initialized
- [ ] All files committed
- [ ] Repository pushed to GitHub/GitLab

### Step 3: Railway Setup

1. **Account Setup**
   - [ ] Railway account created at https://railway.app
   - [ ] GitHub account connected to Railway

2. **Project Creation**
   - [ ] New project created in Railway
   - [ ] Repository connected to Railway
   - [ ] Railway detected Dockerfile automatically

3. **Deployment**
   - [ ] Initial deployment triggered
   - [ ] Build completed successfully
   - [ ] Container started successfully

4. **Domain Configuration**
   - [ ] Custom domain generated
   - [ ] Domain accessible via browser

## ✅ Post-Deployment Verification

### Basic Functionality

Visit your Railway URL (e.g., `https://your-app.railway.app`):

- [ ] **Swagger UI** accessible at `/swagger`
- [ ] **Health Check** - GET `/api/admin/agents` returns `[]` or agent list
- [ ] **Agent Registration** - POST to `/api/agent/register` works
- [ ] **No 500 errors** in Railway logs

### API Endpoints Test

Use Swagger UI or curl to test:

```bash
# Replace YOUR_RAILWAY_URL with your actual URL
export BASE_URL="https://your-app.railway.app"

# Test 1: Get agents
curl $BASE_URL/api/admin/agents

# Test 2: Register agent
curl -X POST $BASE_URL/api/agent/register \
  -H "Content-Type: application/json" \
  -d '{
    "machineName": "TestMachine",
    "ipAddress": "192.168.1.100",
    "macAddress": "AA:BB:CC:DD:EE:FF",
    "operatingSystem": "Windows 11"
  }'

# Test 3: Get registered agents
curl $BASE_URL/api/admin/agents
```

- [ ] All test endpoints respond correctly
- [ ] No authentication errors
- [ ] JSON responses are valid

### SignalR Hub Test

- [ ] SignalR hub accessible at `/adminHub`
- [ ] WebSocket connection succeeds (WSS protocol)
- [ ] Hub accepts connections

### Railway Dashboard Checks

In your Railway project dashboard:

- [ ] **Metrics** - CPU and memory usage normal
- [ ] **Logs** - No errors or exceptions
- [ ] **Deployments** - Status shows "Success"
- [ ] **Networking** - Domain is active

## 🔧 Configuration (Optional)

### Environment Variables

If you need custom configuration:

1. Go to Railway project → Variables
2. Add any required environment variables
3. Redeploy for changes to take effect

Common variables:
- `ASPNETCORE_ENVIRONMENT` (default: Production)
- `ASPNETCORE_URLS` (default: http://+:5030)

### Custom Domain

If you want to use your own domain:

1. Go to Railway project → Settings → Domains
2. Add custom domain
3. Update DNS records as instructed
4. Wait for SSL certificate provisioning

- [ ] Custom domain added (if needed)
- [ ] DNS configured (if needed)
- [ ] SSL certificate active (if needed)

## 📊 Monitoring & Maintenance

### Regular Checks

- [ ] Check Railway logs daily for errors
- [ ] Monitor resource usage (CPU, memory)
- [ ] Review deployment history
- [ ] Check agent connectivity

### Updating the Application

When you need to deploy updates:

```bash
# Make your changes
git add .
git commit -m "Description of changes"
git push origin main
```

Railway will automatically:
- Detect the push
- Build new Docker image
- Deploy with zero downtime

- [ ] Update process tested
- [ ] Automatic deployment working

## 🛡️ Security Checklist (For Production)

**Current Status**: Development/Testing Ready
**For Production**: Additional security needed

- [ ] Add authentication (JWT, API keys)
- [ ] Restrict CORS to specific domains
- [ ] Implement rate limiting
- [ ] Add request validation
- [ ] Use secrets management
- [ ] Enable HTTPS only (Railway default)
- [ ] Add persistent database
- [ ] Implement logging/monitoring
- [ ] Add backup strategy

## 🎯 Next Steps After Deployment

1. **Update Your Agents**
   - Configure agents to use Railway URL instead of localhost
   - Update connection strings in agent configuration
   - Test agent-to-server communication

2. **Documentation**
   - Share Railway URL with team
   - Document API endpoints
   - Create usage guides

3. **Monitoring**
   - Set up error notifications
   - Monitor Railway usage/costs
   - Track API usage patterns

## 📞 Support & Resources

- **Railway Docs**: https://docs.railway.app
- **Railway Discord**: https://discord.gg/railway
- **Railway Status**: https://status.railway.app
- **Project Logs**: Railway Dashboard → Your Project → Logs

## ✨ Success Criteria

Your deployment is successful when:

- ✅ Application builds without errors
- ✅ Container starts and stays running
- ✅ Domain is accessible publicly
- ✅ Swagger UI loads correctly
- ✅ API endpoints respond as expected
- ✅ SignalR hubs are connectable
- ✅ No critical errors in logs
- ✅ Agents can connect and register

---

## 🎉 Congratulations!

Once all items are checked, your AdminServer is successfully deployed on Railway!

**Your app is now:**
- 🌐 Publicly accessible
- 🔄 Automatically deploying updates
- 📊 Monitored by Railway
- 🚀 Ready for production use

**Railway URL**: `https://your-app.railway.app`

---

**Last Updated**: 2024
**Version**: 1.0
**Platform**: Railway + .NET 8
