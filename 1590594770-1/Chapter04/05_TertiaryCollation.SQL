
--Using the TERTIARY_WEIGHTS function to make sorts on tertiary collations more efficient

CREATE TABLE Characters
(
  CharacterString CHAR(3)
    COLLATE SQL_Latin1_General_CP437_CI_AI,
  CharacterWeights AS (TERTIARY_WEIGHTS(CharacterString))
)


CREATE CLUSTERED INDEX IX_Characters
ON Characters 
(
  CharacterString,
  CharacterWeights
)
