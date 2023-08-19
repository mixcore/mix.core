#!/bin/bash

# The 3 variables below store server and login details
# HOST=$1
# USER=$2
# PASSWORD=$3

LOCALFOLDER=$1

REMOTEFOLDER=$2

FILES="./*"

# Go to local current directory
cd "$LOCALFOLDER"
echo $1 $3
# FTP login and upload is explained in paragraph below
ftp -inv $HOST <<EOF
user $USERNAME $PASSWORD

# Go to local current directory
cd "$REMOTEFOLDER"
mput $FILES
mdel app_offline.htm
bye
EOF
