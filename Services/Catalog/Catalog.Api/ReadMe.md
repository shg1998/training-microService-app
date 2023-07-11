To Create Image from dockercompose = > 
- docker-compose -f ./docker-compose.yml -f docker-compose.override.yml up -d
If we want to track mongo data visually (not in commands), we can use mongoclient image :) => (https://hub.docker.com/r/mongoclient/mongoclient)
- for this, we can use "docker run -d -p 3000:3000 mongoclient/mongoclient"