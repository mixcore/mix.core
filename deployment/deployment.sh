#!/bin/bash

# The 3 variables below store server and login details
# HOST=$1
# USER=$2
# PASSWORD=$3

# LOCALFOLDER=$1

# REMOTEFOLDER=$2

FILES="./*"

# Go to local current directory
cd $LOCAL_FOLDER
# echo $HOST $USERNAME $PASSWORD
# FTP login and upload is explained in paragraph below
ftp -inv $FTP_HOST <<EOF
user $FTP_USERNAME $FTP_PASSWORD

# Go to local current directory
cd $REMOTE_FOLDER
mput $FILES
#mdel app_offline.htm
bye
EOF
