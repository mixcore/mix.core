#!/bin/bash

# Check if doctl is installed
if ! command -v doctl &> /dev/null; then
    echo "doctl is not installed. Please install it first: https://docs.digitalocean.com/reference/doctl/how-to/install/"
    exit 1
fi

# Check if DO_TOKEN is set
if [ -z "$DO_TOKEN" ]; then
    echo "Please set your DigitalOcean API token:"
    echo "export DO_TOKEN=your_token_here"
    exit 1
fi

# Set variables
DROPLET_NAME="mixcore"
REGION="sgp1"  # Singapore region
SIZE="s-2vcpu-4gb"  # 2 vCPUs, 4GB RAM
IMAGE="ubuntu-22-04-x64"
SSH_KEY_ID=""  # You need to add your SSH key ID here

# Create droplet
echo "Creating droplet..."
doctl compute droplet create $DROPLET_NAME \
    --region $REGION \
    --size $SIZE \
    --image $IMAGE \
    --ssh-keys $SSH_KEY_ID \
    --wait

# Get droplet IP
DROPLET_IP=$(doctl compute droplet get $DROPLET_NAME --format PublicIPv4 --no-header)

echo "Droplet created successfully!"
echo "IP Address: $DROPLET_IP"

# Wait for SSH to be available
echo "Waiting for SSH to be available..."
sleep 30

# Setup the server
echo "Setting up the server..."
ssh root@$DROPLET_IP << 'EOF'
    # Update system
    apt-get update && apt-get upgrade -y

    # Install required packages
    apt-get install -y \
        apt-transport-https \
        ca-certificates \
        curl \
        gnupg \
        lsb-release \
        software-properties-common

    # Install Docker
    curl -fsSL https://download.docker.com/linux/ubuntu/gpg | gpg --dearmor -o /usr/share/keyrings/docker-archive-keyring.gpg
    echo "deb [arch=$(dpkg --print-architecture) signed-by=/usr/share/keyrings/docker-archive-keyring.gpg] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable" | tee /etc/apt/sources.list.d/docker.list > /dev/null
    apt-get update
    apt-get install -y docker-ce docker-ce-cli containerd.io

    # Install Docker Compose
    curl -L "https://github.com/docker/compose/releases/download/v2.24.5/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
    chmod +x /usr/local/bin/docker-compose

    # Install .NET 9.0 SDK
    wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
    dpkg -i packages-microsoft-prod.deb
    rm packages-microsoft-prod.deb
    apt-get update
    apt-get install -y dotnet-sdk-9.0

    # Create non-root user
    useradd -m -s /bin/bash mixcore
    usermod -aG docker mixcore

    # Setup firewall
    ufw allow 22/tcp
    ufw allow 80/tcp
    ufw allow 443/tcp
    ufw allow 5000/tcp
    ufw allow 5001/tcp
    ufw allow 1433/tcp
    ufw allow 3306/tcp
    ufw allow 6379/tcp
    ufw allow 8080/tcp
    ufw --force enable

    # Create application directory
    mkdir -p /opt/mixcore
    chown -R mixcore:mixcore /opt/mixcore
EOF

echo "Server setup completed!"
echo "You can now SSH into the server using:"
echo "ssh root@$DROPLET_IP"
echo "Or as the mixcore user:"
echo "ssh mixcore@$DROPLET_IP" 