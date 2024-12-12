git checkout main
git pull
docker container start minio
docker stop skyeagle
docker rm skyeagle
docker rmi -f skyeagle
docker build . -t skyeagle:latest -f Dockerfile --no-cache
docker image prune -f
docker run -d --name skyeagle -p 450:5042 -v C:\DockerVolumes\skyeagle:/mnt/ skyeagle:latest
