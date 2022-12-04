from urllib import request
from flask import Flask
import os
import sys
import json
import random

app = Flask(__name__)

@app.route("/")
def principal():

    for root, dirs, archivos in os.walk(r'C:/Users/Kevin Meza/Downloads/EntregableFinalArrolladoraCarritos/servidor'): #os.walk(r’pathname’)

        for archivo in archivos:

            if archivo.endswith('.json'):

                file = open(archivo)
                data = json.load(file)
                result = data
                file.close()

                return result