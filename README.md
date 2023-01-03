# news-portal
A simple news portal with vanilla JS/html for frontend and Azure function App for backend

![news_portal](https://user-images.githubusercontent.com/46394226/210378061-d1afe112-6979-49f5-8f6c-3c8eab1fd2e0.png)

## Frontend
Vanilla JS & HTML

In index.html, update URL of webApi (the backend) that returns JSON from NewsAPI.

## Backend
Azure Function App / Project in C#

In News.cs, update the API Key (NewAPIKey).  Get your API Key from https://newsapi.org

In News.cs, update the "Access-Control-Allow-Origin".  Set this to the url of the fronend/webpage.

