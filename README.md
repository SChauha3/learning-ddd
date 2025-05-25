## IN-PROGRESS
------------------------------------------
## Technology/Tools Used
Visual Studio 2022/ Visual Studio Code
.NET 8
SQL Lite DB

## For adding SQLLite (in memory)
As a result, app.db file will be created in project directory
Install packages:
```
Microsoft.EntityFrameworkCore.Sqlite
Microsoft.EntityFrameworkCore.Tools
```

## Run migrations:
```
dotnet ef --version

If not installed then
dotnet tool install --global dotnet-ef

Navigate to your project folder (where the .csproj is), then run:
dotnet ef migrations add InitialCreate

To generate a SQL script you can run manually or use in CI/CD:
dotnet ef migrations script -o InitialCreate.sql

To apply the migration directly to the database (optional):
dotnet ef database update
```

## Running Application
Application can be run in visual studio and it will launch the OpenAPI url (swagger) at http://localhost:5151/swagger/index.html

## Docker Support
Today I added docker support to the project. Actually I want to deploy this application on my local Kubernetes cluster (Docker Desktop) using helm chart. So I'm trying to publish my image to local first instead of using any image repo like harbor or nexus etc. Below are the commands which I executed to create my image:

```
C:\Projects\greenflux>docker build -t smartcharging -f SmartCharging.Api/Dockerfile .
C:\Projects\greenflux>docker run -p 8080:8080 -e ASPNETCORE_ENVIRONMENT=Development smartcharging

```

## Helm Chart (windows with Docker Desktop):
```
#Install Helm
C:\Projects\greenflux>choco install kubernetes-helm
#Helm version
C:\Projects\greenflux>helm version
#Kubectl version
C:\Projects\greenflux>kubectl version --client

#Expose health endpoint. It's mandatory for kubernetes deployment.
app.MapGet("/health/live", () => Results.Ok("Alive"));
app.MapGet("/health/ready", () => Results.Ok("Ready"));

#Now we are done with pre-requisites.
#Create a custom chart.
C:\Projects\greenflux>cd Deployment
C:\Projects\greenflux>helm create smartcharging

This creates a scaffold with:
Chart.yaml
values.yaml
templates in templates/

#Pass the variables in values.yaml
#install with helm
helm install smartcharging ./smartcharging

C:\Projects\greenflux>
C:\Projects\greenflux>
```

As I don't have any image registry so in order to run helm on local, few extra steps needs to be done which are below.
```
#Create registry container
docker run -d -p 5001:5001 --name registry registry:2
#Tag and Push image to docker registery.
docker tag smartcharging localhost:5001/smartcharging
docker push localhost:5000/smartcharging
#Set helm values as below:
image:
  repository: localhost:5001/smartcharging
  tag: latest
  pullPolicy: Never

#Deploy with helm
helm install smartcharging ./smartcharging

#To uninstall
helm uninstall smartcharging

#To access application in browser
kubectl get pods

#port forwarding (optional if ingress is disabled)
kubectl port-forward smartcharging-7f98447897-5l4ll 8080:8080

#If you don't want to use port farward for services then ingress coontroller can handled the incoming traffic. Below needs to be done for this. Update values.yaml file like Below.
ingress:
  enabled: true
  className: "nginx"
  annotations: {}
    # kubernetes.io/ingress.class: nginx
    # kubernetes.io/tls-acme: "true"
  hosts:
    - host: smartcharging.local
      paths:
        - path: /
          pathType: ImplementationSpecific

#Install nginx ingress controller
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.9.4/deploy/static/provider/cloud/deploy.yaml

#Wait for controller to be ready:
kubectl get pods -n ingress-nginx

#Edit your local /etc/hosts file
127.0.0.1 smartcharging.local

#Deploy or Upgrade your Helm release
helm upgrade --install smartcharging ./your-chart-path -f values.yaml

#Other helping commands for issues
kubectl get svc
```

Application should be accessible at http://smartcharging.local/health/live