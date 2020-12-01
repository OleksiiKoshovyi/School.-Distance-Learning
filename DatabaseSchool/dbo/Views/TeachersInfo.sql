CREATE VIEW TeachersInfo AS
SELECT TeacherId, CONCAT(FirstName, ' ', Patronymic) AS TeacherPartName, CONCAT(SurName, ' ', FirstName, ' ', Patronymic) AS TeacherFullName
FROM Teachers;