from django.conf.urls import url
from whiteboard import views

urlpatterns = [
    url(r'^createAccount/$', views.createAccount),
    url(r'^authenticate/$', views.authenticateuser),
    url(r'^createBoard/$', views.createNewBoard),
    url(r'^getUserBoards/$', views.getUserBoards),
    url(r'^getBoardUsers/$', views.getBoardUsers),
    url(r'^addRole/$', views.addRole),
]
