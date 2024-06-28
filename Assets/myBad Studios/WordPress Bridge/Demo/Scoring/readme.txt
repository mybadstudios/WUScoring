ABOUT THIS DEMO
===============

This demo demonstrates how to use the "out of the box" demo leaderboards prefab

NOTE: THIS DEMO REQUIRES THE LOGIN PREFAB TO BE PRESENT IN THE SCENE. 
IF YOU DON'T HAVE IT IN THE SCENE, DRAG THE WPServer AND WULoginCanvasNoServer PREFABS INTO THE SCENE ROOT
AND SET THE Diplay Order on WULoginCanvasNoServer's Canvas TO 1

THE THEORY BEHIND IT ALL
========================

By default, in order to submit a high score you need only ever call 
	WUScoring.SubmitScore(myScore) ;
...at the end of a level and that is all there is to it, but by popular demand this asset was modified to
allow games to optionally have multiple leaderboards. This demo showcases this feature by calling SubmitScore()
with not only the score but also the optional second parameter, ID.

If you do not supply this second parameter then WUScoring defaults to using your game's actual ID and will
correctly store player's scores for the actual game. When you provide the second parameter you can override this
ID to any value you want and in so doing create as many leaderboards as you want.

HANDS ON: Running the demo
======================

Only after you have logged in you will you be able to make use of the demo so make sure the login kit is installed
and the login prefab is in the scene before you hit play.

When the scene starts you will be presented with a button that will pick a random number and submit that as
your fictional high score. If the number is larger than your existing score it will replace it. 
If the number is lower than your existing high score it will be discarded. In either event it will trigger
the function to fetch an display the scores by spawning an instance of a prefab created for this purpose.
You can use this prefab in your own games as it was made to be easily skinnable but, when it is all said and done,
this is merely a demo prefab showing you one way of displaying high scores. You are free to create your own.

Ultimately, this kit requires that you learn only 2 functions. These functions can be called from any script without
needing any references to any scripts or game objects. These functions trigger Actions that allow you to run custom
code and in this demo we register to these events to instantiate the login prefab. As stated before, this is only
a demonstration of one way how you can use this kit and you are free to respond to the events in completely different
ways if you want to but by doing it this way you can use the existing prefab for an "out of the box" solution that
will have you up and running with leaderboards within minutes!

The two functions you need to learn are:
1. WUScoring.SubmitScore(NewScore);
2. WUScoring.FetchScores(HowMany);
See WUScoring.cs for detailed instructions on the available Actions, what they do and when/how to use them!

Back to the demo...
You are also presented with a button to fetch the scores manually. To submit a score or to fetch a list of scores, 
simply click the relevant button and you are done.

Below these two buttons is a field where you can specify the leaderboard id. For the sake of this demo
set the value to 1 and then click on the "Submit Score" button a few times. Notice how your name appears
at the top of the list and how your score only ever goes up. As stated before, if your new random score
is lower than your current high score then it is ignored and your real high score is retained.

At some time, change this ID value to 2 and start submitting scores again. The same thing will happen only
this time this is saved to a new leaderboard. If you have multiple accounts on your website you can sign in using 
a differnt account and run this demo again. You will now see both your account's scores in the list.

Toggle the Id value between 1 and 2 and then click on "Fetch Scores" to fetch/view the relevant leaderboard and
verify you did actually create multiple leaderboards.

WARNING
=======

The default behaviour of this kit is to store the high scores for the current game and thus doesn't require
you to manually specify a leaderboard ID. When you fetch the scores it will automatically fetch the right ones.

When you specify the ID manually you are free to use any absolute integer you want but then the Id is not
validated or verified. It is up to you to make sure the IDs are unique across all your games. 

For example: If you have 3 different games on your website and their ID's are 1, 10 and 100 (for example)
and then you tell game 10 to submit scores using ID's 1, 2 and 3 (for example) then game 1 and game 10 will
share the leaderboard with ID 1. Both games will submit and display scores to/from the same leaderboard and
will thus render that leaderboard useless. 

So, if you specify leader board IDs manually, be very careful to ensure they are unique not only to each other
within the same game but across all leaderboard IDs for all games on your website.
