from django.db import models

class Board(models.Model):
	boardid = models.AutoField(primary_key=True)
	userid = models.CharField()
	role = models.CharField()
