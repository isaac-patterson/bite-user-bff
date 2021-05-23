# Info

Serves the iOS user app, android user app and user webapp

Sits within private subnets in an ECS cluster with the retail BFF. Runs on AWS fargate EC2 instances

# Development and deployment

Merging to master will trigger AWS Code Pipeline which runs tests and deploys to Fargate (production)

Develop features on a branch and then make a pull request to master
Comment on all commits with the jira tag ```git commit -m "[VENI-10] updating readme."```