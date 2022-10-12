using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTutor : MonoBehaviour
{
	[SerializeField] protected List<GameObject> ballPrefabs;
	[SerializeField] protected GameObject cueBallPrefab;
	[SerializeField] protected Table table;
	protected List<Ball> balls;
	public GameObject taptocontinue;
	bool isEnableSkip = false;
	protected List<Ball> Balls
	{
		get
		{
			if (balls == null)
			{
				balls = new List<Ball>();
				for (int i = 1; i < GameManager.BallsCount; i++)
				{
					balls.Add(GameManager.GetBall(i));
				}
			}

			return balls;
		}
	}

	public Ball CueBall
	{
		get
		{
			return GameManager.GetBall(0);
		}
	}
	
	// Start is called before the first frame update
	void Start()
    {



		//taptocontinue.SetActive(false);
		isEnableSkip = false;
		//StartCoroutine(iStart());
        CreateBalls();
        RackBalls();
        Debug.Log("Hello world");
        
    }
	IEnumerator iStart()
	{
		yield return new WaitForSeconds(1f);
		taptocontinue.SetActive(true);
		isEnableSkip = true;
	}
	public Ball GetBall(int ballNumber)
	{
		if (ballNumber < 0 || ballNumber > Balls.Count)
		{
			return null;
		}

		if (ballNumber == 0)
		{
			return CueBall;
		}

		return Balls[ballNumber - 1];
	}

	public Ball GetRandomBall()
	{
		int randomType = Random.Range(0, 2);
		if (randomType == 1)
		{
			return GetRandomBall(BallType.SOLID);
		}
		else
		{
			return GetRandomBall(BallType.STRIPED);
		}
	}

	public Ball GetRandomBall(BallType type)
	{
		if (type == BallType.CUE)
		{
			return CueBall;
		}

		if (type == BallType.EIGHT)
		{
			return GetBall(8);
		}

		if (type == BallType.SOLID)
		{
			int randomBallNumber = Random.Range(1, 8);
			return GetBall(randomBallNumber);
		}

		if (type == BallType.STRIPED)
		{
			int randomBallNumber = Random.Range(9, 16);
			return GetBall(randomBallNumber);
		}

		return null;
	}

	private List<Ball> RandomizeBalls(List<Ball> balls, List<Ball> except = null)
	{
		List<Ball> randomizedBalls = PoolUtils.Shuffle(balls);

		if (except != null)
		{
			foreach (Ball ball in except)
			{
				randomizedBalls.Remove(ball);
			}
		}

		return randomizedBalls;
	}
	protected void RackBalls()
	{
		List<Ball> ballsToSkip = new List<Ball>();
		ballsToSkip.Add(GetBall(8));

		Ball topCornerBall = GetRandomBall();
		Ball bottomCornerBall;
		if (topCornerBall.Type == BallType.SOLID)
		{
			bottomCornerBall = GetRandomBall(BallType.STRIPED);
		}
		else
		{
			bottomCornerBall = GetRandomBall(BallType.SOLID);
		}

		ballsToSkip.Add(topCornerBall);
		ballsToSkip.Add(bottomCornerBall);

		List<Ball> shuffledBalls = RandomizeBalls(Balls, ballsToSkip);

		Vector3 ballPosition = new Vector3();
		int k = 0;
		for (int i = 0; i < 5; i++)
		{
			ballPosition.y = table.FootSpot.position.y;

			float deltaX = Balls[i].Radius * 1.8f;
			ballPosition.x = table.FootSpot.position.x + (deltaX * i);

			float deltaZ = Balls[i].Radius * 1.05f;
			ballPosition.z = table.FootSpot.position.z + (deltaZ * i);

			for (int j = 0; j < i + 1; j++)
			{
				Ball ballToPlace;
				// Place 8-ball
				if (i == 2 && j == 1)
				{
					ballToPlace = GetBall(8);
				}
				// Place top corner ball
				else if (i == 4 && j == 0)
				{
					ballToPlace = topCornerBall;
				}
				// Place bottom corner ball
				else if (i == 4 && j == 4)
				{
					ballToPlace = bottomCornerBall;
				}
				// Place other balls
				else
				{
					ballToPlace = shuffledBalls[k];
					k++;
				}

				ballToPlace.transform.position = ballPosition;
				ballPosition.z -= (deltaZ * 2);
			}
		}
	}



	// Update is called once per frame
	void Update()
    {
        
    }


	public void OnClick_NextLevelButton()
	{
		if(isEnableSkip)
		PoolSceneManager.Instance.MyLoadScene("MainMenu");
	
	}
 void CreateBalls()
    {
        GameObject cueBallObj = Instantiate(cueBallPrefab);
        cueBallObj.transform.position = table.HeadSpot.position;

        for (int i = 0; i < ballPrefabs.Count; i++)
        {
            GameObject ballObj = Instantiate(ballPrefabs[i]);
            ballObj.transform.rotation = Random.rotation;
        }
    }
}
