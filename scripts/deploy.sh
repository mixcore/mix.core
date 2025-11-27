#!/bin/bash

# Check if SSH key and droplet IP are provided
if [ -z "$1" ] || [ -z "$2" ]; then
    echo "Usage: ./deploy.sh <path_to_ssh_key> <droplet_ip>"
    exit 1
fi

SSH_KEY=$1
DROPLET_IP=$2

# Build the application
echo "Building the application..."
docker-compose build

# Create deployment archive
echo "Creating deployment archive..."
tar -czf deploy.tar.gz \
    docker-compose.yml \
    Dockerfile \
    .dockerignore \
    src/

# Copy files to server
echo "Copying files to server..."
scp -i $SSH_KEY deploy.tar.gz mixcore@$DROPLET_IP:/opt/mixcore/

# Deploy on server
echo "Deploying on server..."
ssh -i $SSH_KEY mixcore@$DROPLET_IP << 'EOF'
    cd /opt/mixcore
    tar -xzf deploy.tar.gz
    rm deploy.tar.gz
    
    # Start the application
    docker-compose up -d
    
    # Check if containers are running
    docker-compose ps
EOF

echo "Deployment completed!"
echo "Application is running at:"
echo "http://$DROPLET_IP:5000"
echo "phpMyAdmin: http://$DROPLET_IP:8080" 