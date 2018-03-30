// Write your JavaScript code.

$(function () {
    $("#BaseTree").jstree(
        {
            "types": {
                "team": {
                    "icon": "fas fa-users",
                    "valid_children": ["student"]
                },
                "student": {
                    "icon": "fas fa-user",
                    "valid_children": "none"
                }
            },
            "conditionalselect": function (node) {
                return node.type !== "team";
            },
            "plugins": ["conditionalselect", "sort", "wholerow", "unique", "types"]
        }
    );
});
$("#BaseTree")
    .on("changed.jstree", function (e, data) {
        var parameter = { "id": data.node.data.jstree.id };
        $.getJSON("/Home/Student", parameter, onStudentLoaded);
    });

function onStudentLoaded(student) {
    $("#SelectedStudent").text(student.fullName);
    $("#CurrentStudentLastName").text(student.lastName);
    $("#CurrentStudentFirstName").text(student.firstName);
    $("#CurrentStudentMiddleName").text(student.middleName);
    $("#CurrentStudentGroup").text(student.group);
    $("#CurrentStudentTeam").text(student.team.number);
    $("#CurrentStudentAverageValue").text(student.averageValue);
}
