README

1. The first step of this project was to extract data. For this tweepy library of twitter was used for data extraction. Tweepy gives Cursor API to get data.
e.g. Firstly you will need keys to get twitter data.
consumer_key = ""
consumer_secret = ""
access_token = ""
access_token_secret = ""
Then you can use tweepy APIs for connection.
2. After data extraction the data was stored into the mongodb database.
e.g. For this pymongo library was used to connect to mongodb server.
3. After getting the data using twitter APIs then a flask App was created to create a web application.
4. Then a route was created on Flask for displaying the line graph for the tweets per day. 
For plotting this graph pandas and matplotlib was used. Firstly, the data was stored into a list from
mongodb database. The the data was filtered using pandas library and then matplotlib was used to plot the
graph. The graph created was saved as an image file and then its URL was sent as an <img src="" /> to
the HTML page.
5. Similar thing was done for the pie chart graph.

To deploy this Flask App on Heroku
1. You will need Flask, gunicorn, a Heroku account and the HerokuCLI. Also, pip should be installed.
2. Then we need the requirements used for the app. This can be done by pip freeze > requirements.txt
3. The last thing that we need is a Procfile. In it’s simplest form you put one process per line that you want to be ran on your Heroku environment.
web: gunicorn app:app
4. Then we need to connect our app to heroku using heroku login and then push our code to heroku.


Challenges Faced 
1. Twitter API does not allow mining large amounts of data. It will kill you request after some time. So,
I mined the data day by day.
2. The data from twitter does not have location for all the tweets as it is private info. Only some tweets
will have location and the location can be like Hyderabad, India or India or Bangalore, India. So, we have to
process these together using Pandas.
3. Deploying this application on Heroku is also a big challenge. Firstly, we need a mongodb database. 
Heroku does provides paid mongodb database which requires credit card information. So, I used mLab free 
mongodb database and indexed all my twitter data there.
4. After, setting up mongodb we need to deploy the app. For this the requirements should be compatible with
each other otherwise installation will fail or many times downloading breaks in between leading to 
faulty installations. In this case you need to reinstall again.
5. After, uploading code on heroku you need to change the syntax of your code to make it compatible to
heroku. It can give errors that you won't face in your localhost.

