# Railway Deployment Guide for AdminServer

This guide will walk you through deploying the AdminServer to Railway.app.

## 📋 Prerequisites

1. **Railway Account**: Sign up at https://railway.app (free tier available)
2. **Git Repository**: Your code must be in a Git repository (GitHub, GitLab, or Bitbucket)
3. **Git Installed**: Make sure Git is installed on your local machine

## 🚀 Quick Deployment Steps

### Step 1: Initialize Git Repository (if not already done)

```bash
cd c:\Users\ASUS\Desktop\AdminServer
git init
git add .
git commit -m "Initial commit for Railway deployment"
```

### Step 2: Push to GitHub/GitLab

Option A - Create a new repository on GitHub:
1. Go to https://github.com/new
2. Create a new repository (e.g., "AdminServer")
3. Don't initialize with README (we already have one)

Then push your code:
```bash
git remote add origin https://github.com/YOUR_USERNAME/AdminServer.git
git branch -M main
git push -u origin main
```

Option B - Use Railway CLI:
```bash
# Install Railway CLI
npm i -g @railway/cli

# Login to Railway
railway login

# Initialize and deploy
railway init
railway up
```

### Step 3: Deploy on Railway (Web Interface)

1. **Login to Railway**
   - Go to https://railway.app
   - Sign in with your GitHub account

2. **Create New Project**
   - Click "New Project"
   - Select "Deploy from GitHub repo"
   - Choose your AdminServer repository
   - Railway will automatically detect the Dockerfile

3. **Wait for Deployment**
   - Railway will:
     - Clone your repository
     - Build the Docker image
     - Deploy the container
     - Assign a public URL
   - This usually takes 2-5 minutes

4. **Get Your URL**
   - Once deployed, click on your project
   - Go to "Settings" → "Networking"
   - Click "Generate Domain"
   - Your app will be available at: `https://your-app.railway.app`

## 🔧 Configuration

### Environment Variables (Optional)

Railway automatically provides:
- `PORT` - The port your app should listen on (automatically configured)

You can add custom environment variables in Railway:
1. Go to your project
2. Click "Variables"
3. Add any needed variables

### Health Checks

Railway will automatically monitor your application. You can verify it's running:
- Visit: `https://your-app.railway.app/swagger`
- Check the API: `https://your-app.railway.app/api/admin/agents`

## 📊 Accessing Your Deployed API

Once deployed, you can access:

- **Swagger UI**: `https://your-app.railway.app/swagger`
- **API Base URL**: `https://your-app.railway.app/api/`
- **SignalR Hub**: `wss://your-app.railway.app/adminHub`
- **Agent Hub**: `wss://your-app.railway.app/agentHub`

### Example API Calls

```bash
# Get all agents
curl https://your-app.railway.app/api/admin/agents

# Register a new agent
curl -X POST https://your-app.railway.app/api/agent/register \
  -H "Content-Type: application/json" \
  -d '{
    "machineName": "TestAgent",
    "ipAddress": "192.168.1.100",
    "macAddress": "00:11:22:33:44:55",
    "operatingSystem": "Windows 11"
  }'
```

## 🐛 Troubleshooting

### Deployment Fails

1. **Check Build Logs**
   - Go to Railway dashboard
   - Click on your project
   - View the "Deployments" tab
   - Check the build logs

2. **Common Issues**:
   - Dockerfile not found: Ensure Dockerfile is in the root directory
   - Build timeout: Railway free tier has build time limits
   - Port issues: The app uses the PORT environment variable automatically

### App Not Starting

1. **Check Runtime Logs**
   - In Railway dashboard, view "Logs"
   - Look for errors or exceptions

2. **Verify Dockerfile**:
   - Make sure Dockerfile is using .NET 8 runtime
   - Check that the app is binding to `0.0.0.0:$PORT`

### Connection Issues

1. **CORS Problems**:
   - The app is configured with `AllowAnyOrigin` for development
   - For production, update CORS settings in `Program.cs`

2. **SignalR WebSocket Issues**:
   - Railway supports WebSockets by default
   - Ensure your client uses WSS (not WS) for the deployed URL

## 🔄 Updating Your Deployment

To deploy updates:

```bash
# Make your changes
git add .
git commit -m "Description of changes"
git push origin main
```

Railway will automatically:
- Detect the push
- Rebuild the Docker image
- Deploy the new version
- Zero-downtime deployment (if possible)

## 💰 Cost Considerations

**Railway Free Tier**:
- $5 free credit per month
- Automatically sleeps after inactivity
- Limited to 500 hours of usage
- Perfect for development/testing

**Railway Pro Plan** ($20/month):
- Unlimited projects
- More resources
- Priority support
- Better for production

## 📈 Monitoring

Railway provides:
- **Metrics**: CPU, Memory, Network usage
- **Logs**: Real-time application logs
- **Deployments**: History of all deployments
- **Analytics**: Request metrics (on Pro plan)

Access these in your Railway project dashboard.

## 🔒 Security Recommendations

Before going to production:

1. **Add Authentication**
   - Implement JWT or API key authentication
   - Protect sensitive endpoints

2. **Update CORS**
   ```csharp
   // In Program.cs
   options.AddDefaultPolicy(policy =>
       policy.WithOrigins("https://yourdomain.com")
             .AllowAnyHeader()
             .AllowAnyMethod());
   ```

3. **Add Rate Limiting**
   - Prevent abuse
   - Consider using ASP.NET Core rate limiting middleware

4. **Use HTTPS Only**
   - Railway provides HTTPS by default
   - Ensure your agents connect via HTTPS

5. **Add Persistent Storage**
   - Railway offers database add-ons
   - Consider PostgreSQL or MongoDB
   - Replace in-memory storage

## 📚 Additional Resources

- Railway Documentation: https://docs.railway.app
- Railway Community: https://help.railway.app
- Railway Discord: https://discord.gg/railway
- .NET on Railway: https://docs.railway.app/guides/dotnet

## ✅ Deployment Checklist

- [ ] Git repository created and code committed
- [ ] Railway account created
- [ ] Project connected to Railway
- [ ] Deployment successful
- [ ] Domain generated
- [ ] Swagger UI accessible
- [ ] API endpoints working
- [ ] SignalR hubs connectable
- [ ] Logs checked for errors
- [ ] CORS configured for your needs
- [ ] Environment variables set (if needed)

## 🆘 Support

If you encounter issues:

1. Check Railway's status page: https://status.railway.app
2. Review deployment logs in Railway dashboard
3. Check the app logs for errors
4. Visit Railway Discord for community support
5. Review this guide's troubleshooting section

## 🎉 Success!

Once deployed, your AdminServer will be:
- ✅ Publicly accessible
- ✅ Running on Railway's infrastructure
- ✅ Automatically restarting on crashes
- ✅ Monitoring system metrics
- ✅ Ready to accept agent connections

Your agents can now connect to your Railway URL instead of localhost!

---

**Next Steps**: Configure your agents to use the Railway URL as their server endpoint.
