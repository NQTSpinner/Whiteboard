from django.shortcuts import render
from django.contrib.auth.models import User
from django.utils import simplejson
from django.http import HttpResponse
from django.contrib import auth
from models import Board

class JsonResponse(HttpResponse):
    """
        JSON response
    """
    def __init__(self, content, mimetype='application/json', status=None, content_type=None):
        super(JsonResponse, self).__init__(
            content=simplejson.dumps(content),
            mimetype=mimetype,
            status=status,
            content_type=content_type,
        )


def createAccount(request):
	newUsername = request.GET.get('user')
	newPassword = request.GET.get('pass')

	try:
		user = User.objects.create_user(username = newUsername, password = newPassword)
		issuccess = True

	except:
		issuccess = False

	return JsonResponse({'issuccess':issuccess})



def authenticateuser(request):
	username = request.GET.get('user')
	password = request.GET.get('pass')

	try:
		user = User.objects.get(username__exact=username)

	except:
		user = None

	if user == None:
		authenticated = "User Doesnt Exist"

	elif user.check_password(password):
		authenticated = True

	else:
		authenticated = False

	return JsonResponse({'authenticated':authenticated})



def createNewBoard(request):
	username = request.GET.get('user')
	role = "Admin"

	board = Board(userid=username,role=role)

	board.save()

	bId = board.boardid

	issuccess = True

	return JsonResponse({'boardId':bId,'issuccess':issuccess})



def getUserBoards(request):

	username = request.GET.get('user','NONE')
	boards = Board.objects.filter(userid__exact=username)
	returnDict = {}

	for i in boards.all():
		returnDict[i.boardid] = i.role

	return JsonResponse(returnDict)



def addRole(request):

	admin = request.GET.get('admin')
	newuser = request.GET.get('newuser')
	rolein = request.GET.get('role')
	boardrequest = request.GET.get('boardid')

	board = Board(boardid = boardrequest, userid = newuser, role = rolein)

	filtered = Board.objects.filter(boardid__exact=boardrequest)

	if filtered.filter(userid__exact=admin).exists():
		board.save()
		return JsonResponse({'issuccess':True})

	else:
		return JsonResponse({'issuccess':False})

def getBoardUsers(request):
	boardid = request.GET.get('boardid')
	boards = Board.objects.filter(boardid__exact=boardid)
	returnDict= {}

	for i in boards.all():
		returnDict[i.userid]=i.role

	return JsonResponse(returnDict)







