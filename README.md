# Info

Serves the iOS user app, android user app and user webapp

Sits within private subnets in an ECS cluster with the retail BFF. Runs on AWS fargate EC2 instances

# Development and deployment

Merging to master will trigger AWS Code Pipeline which runs tests and deploys to Fargate (production). 

Develop features on a branch and then make a pull request to master
Comment on all commits with the jira tag ```git commit -m "[BITE-10] updating readme."```


You can moniter the realease on [AWS Codepipeline](https://console.aws.amazon.com/codesuite/codepipeline/pipelines?region=us-east-1&pipelines-meta=eyJmIjp7InRleHQiOiIifSwicyI6eyJwcm9wZXJ0eSI6InVwZGF0ZWQiLCJkaXJlY3Rpb24iOi0xfSwibiI6MTAsImkiOjB9)

# Hitting from postman

To access the endpoint on postman 

* ```dotnet run``` to run the api
* Hit this endpoint for local ```localhost:5000/user/api/ENDPOINT``` or ```https://biteapp.work/user/api/ENDPOINT``` for prod
* In the Authorization tab of postman, select OAuth 2.0 and insert these details
  * Token name: ```user token```
  * Grant type: ```Implicit```
  * Callback URL: ```https://biteapp.work/oauth2/idpresponse```
  * Auth URL: ```https://bite-user-auth.auth.us-east-1.amazoncognito.com/login```
  * Client Id: ```c2skur0q78bdmihgl2hif1nlc```
  * Scope: ```openid```
  * State:
  * Client Authentication: ```Send as Basic Auth Header```
* Click "Get New Access Token" and login with you bite user credentials
* Click available tokens -> manage tokens 
* Copy the id_token, close window and paste it into the Access token field
* Type ```Bearer``` in the Header Prefix
* Send request!