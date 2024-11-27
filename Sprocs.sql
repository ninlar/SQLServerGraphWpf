CREATE PROCEDURE [dbo].[GetGraph]
AS
BEGIN

	SELECT Id, [Name]
	FROM Person

	SELECT Id, [Name]
	FROM City

	SELECT Person1.ID, Person2.ID
	FROM Person person1, Person person2, friendOf
	WHERE MATCH(person1-(friendOf)->person2)

	select Person1.ID, City1.Id
	FROM Person person1, livesin, City city1
	where MATCH(person1-(livesIn)->city1)
END

exec GetGraph

SELECT
    Person1.name AS PersonName,
    STRING_AGG(Person2.name, '->') WITHIN GROUP (GRAPH PATH) AS Friends,
    Restaurant.name
FROM
    Person AS Person1,
    friendOf FOR PATH AS fo,
    Person FOR PATH  AS Person2,
    likes,
    Restaurant
WHERE MATCH(SHORTEST_PATH(Person1(-(fo)->Person2){1,3}) AND LAST_NODE(Person2)-(likes)->Restaurant )
AND Person1.name = 'Jacob'
AND Restaurant.name = 'Ginger and Spice'


 SELECT PersonName, Friends, levels
FROM (
    SELECT
        Person1.name AS PersonName,
        STRING_AGG(Person2.name, '->') WITHIN GROUP (GRAPH PATH) AS Friends,
        LAST_VALUE(Person2.name) WITHIN GROUP (GRAPH PATH) AS LastNode,
        COUNT(Person2.name) WITHIN GROUP (GRAPH PATH) AS levels
    FROM
        Person AS Person1,
        friendOf FOR PATH AS fo,
        Person FOR PATH  AS Person2
    WHERE MATCH(SHORTEST_PATH(Person1(-(fo)->Person2)+))
    AND Person1.name = 'Jacob'
) AS Q
WHERE Q.LastNode = 'Alice'

   SELECT Person1.name AS Friend1, Person2.name AS Friend2
   FROM Person Person1, friendOf friend1, Person Person2, 
        friendOf friend2, Person Person0
   WHERE MATCH(Person1-(friend1)->Person0<-(friend2)-Person2);

    SELECT Person1.name AS Friend1, Person2.name AS Friend2
    FROM Person Person1, friendOf friend1, Person Person2, 
         friendOf friend2, Person Person0
    WHERE MATCH(Person1-(friend1)->Person0<-(friend2)-Person2);

    SELECT Person2.name AS FriendName
    FROM Person Person1, friendOf friend, Person Person2 
    WHERE MATCH(Person2<-(friend)-Person1);

    SELECT Person1.name AS Friend1, Person2.name AS Friend2
    FROM Person Person1, friendOf friend1, Person Person2, 
         friendOf friend2, Person Person0
    WHERE MATCH(Person1-(friend1)->Person0 AND Person2-(friend2)->Person0);