from flask import Flask, render_template
from flask_bootstrap import Bootstrap
import tweepy
import json
from pymongo import MongoClient
from datetime import datetime
import pandas as pd
import pyparsing
import matplotlib.pyplot as plt
import io
import base64
from odo import odo

app = Flask(__name__)
Bootstrap(app)
MONGO_HOST= 'mongodb://root:root123@ds161742.mlab.com:61742/twitter_db'
consumer_key = "2NAGFGRpb0jwHinsioMAt7tqb"
consumer_secret = "s48FJkeQ2aTC3Ze6UZsa5Fhb7p2tzjqwYFblqzL5U8tdA6Shne"
access_token = "885662530525253634-8fUqxEPaO7id39S2fpFO3xDCQVg8lmn"
access_token_secret = "a5xtmVCyDiFpX4GYC47z9uzHtJL4tVlyTyjkv2TtCYtYJ"
date1 = datetime.strptime("07/07/17", "%d/%m/%y")
date2 = datetime.strptime("17/07/17", "%d/%m/%y")
	
@app.route("/")
def homePage():
    return render_template('index.html')
	
@app.route("/extract_data")
def extractData():
	auth = tweepy.OAuthHandler(consumer_key, consumer_secret)
	auth.set_access_token(access_token, access_token_secret)
	api = tweepy.API(auth)
	
	client = MongoClient(MONGO_HOST)
	db = client.twitter_db 
	collection = db.my_collection
	
	for tweet in tweepy.Cursor(api.search,q="#analytics",count=100,\
                           lang="en",\
                           since="2017-07-7",until="2017-07-8").items():
		data ={}
		data['created_at'] = tweet.created_at
		data['geo'] = tweet.geo
		data['id'] = tweet.id
		data['text'] = tweet.text
		data['location'] = tweet.user.location

		collection.insert(data)
	
	
	return "Hello world"
	
@app.route("/displaygraph")
def displayGraph():
	client = MongoClient(MONGO_HOST)
	db = client.twitter_db 
	collection = db.my_collection
	
	img = io.BytesIO()
	
	tweets_data = []
	count = 0
	
	for dat in collection.find({'created_at':{"$gte":date1, "$lt":date2}}):
		tweets_data.append(dat)
		count = count + 1
		
	data_frame = pd.DataFrame(tweets_data) 

	df = pd.DataFrame({'date':data_frame['created_at'].dt.date.unique()})

	#frequency of dates or the number of tweets that have occured that date
	#y axis will have frequency
	y_axis = data_frame['created_at'].dt.date.value_counts()

	fig, ax = plt.subplots()
	ax.tick_params(axis='x', labelsize=8)
	ax.tick_params(axis='y', labelsize=10)
	ax.set_xlabel('Dates', fontsize=15)
	ax.set_ylabel('Number of tweets' , fontsize=15)
	ax.set_title('Number of Tweets Per Day with #analytics', fontsize=15, fontweight='bold')

	y_axis.plot(ax=ax, kind='line', color='red')

	plt.savefig(img, format='png')
	img.seek(0)

	plot_url = base64.b64encode(img.getvalue()).decode()

	return '<img src="data:image/png;base64,{}">'.format(plot_url)
	
@app.route("/displaypie")
def displayPie():
	client = MongoClient(MONGO_HOST)
	db = client.twitter_db 
	collection = db.my_collection
        
	img = io.BytesIO()

	tweets_data = []

	for dat in collection.find({'created_at':{"$gte":date1, "$lt":date2}}):
		tweets_data.append(dat)
		
	#tweets = pd.DataFrame()
	data_frame = pd.DataFrame(tweets_data)
	
	#data_frame['location'] = map(lambda tweet: tweet['location']if tweet['location'] != '' else '', tweets_data)
	#tweets.sort_values(by=['date'], ascending=[True])
	location_df = data_frame['location'].apply(lambda x: pd.Series(x.split(',')))
	print(location_df)

        
	#renaming the columns to city, state, country.

	location_df.rename(columns={0:'City',1:'Country',2:'Country2'},inplace=True)
	location_df.Country.fillna(location_df.City, inplace=True)
	location_df['Country'] = location_df['Country'].map(lambda x: x.strip())
	z_axis = 100. * location_df['Country'].str.lower().value_counts()/len(location_df['Country'])
	z_axis[:20].plot(kind='pie', autopct='%1.0f%%' , subplots=True)
	plt.axis('equal')
	plt.title('Tweets distribution')
	plt.savefig(img, format='png')
	img.seek(0)

	plot_url = base64.b64encode(img.getvalue()).decode()

	return '<img src="data:image/png;base64,{}">'.format(plot_url)
	
        
	
if __name__ == "__main__":
	app.run()
