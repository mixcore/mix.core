#!/bin/bash

# The 3 variables below store server and login details
HOST=$1
USER=$2
PASSWORD=$3


# $1 is the first argument to the script
# We are using it as upload directory path
# If it is '.', file is uploaded to current directory.
SOURCE=$4

DESTINATION=$5

FILES="./*"

# Go to local current directory
cd "$SOURCE"

# FTP login and upload is explained in paragraph below
ftp -inv $HOST <<EOF
user $USER $PASSWORD

# Go to local current directory
cd "$DESTINATION"
mput $FILES
mdel app_offline.htm
bye
EOF