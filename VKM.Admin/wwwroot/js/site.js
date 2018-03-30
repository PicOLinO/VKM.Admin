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

function onStudentLoaded(view) {
    $("#SelectedStudent").text(view.student.fullName);

    $("#CurrentStudentLastName").text(view.student.lastName);
    $("#CurrentStudentFirstName").text(view.student.firstName);
    $("#CurrentStudentMiddleName").text(view.student.middleName);
    $("#CurrentStudentGroup").text(view.student.group);
    $("#CurrentStudentTeam").text(view.student.team.number);
    $("#CurrentStudentAverageValue").text(view.student.averageValue);

    $('#CurrentStudentHistory').empty();
    $.each(view.history,
        function (index, value) {
            $("#CurrentStudentHistory")
                .append('<tr>' +
                            '<td>' + value.date +
                            '</td>' +
                            '<td>' + value.value +
                            '</td>' +
                            '<td>' + value.algorithmName +
                            '</td>' +
                        '</tr>');
        });

    var studentContentControl = document.getElementById("StudentContent");
    studentContentControl.style.visibility = "visible";
}
