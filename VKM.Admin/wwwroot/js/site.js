var jstreeSelectedItem = {id: -1, type: undefined};

$(function () {
    $("#BaseTree").jstree(
        {
            "core": {
                check_callback: true,
                multiple: false
            },
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
            //"conditionalselect": function (node) {
            //    return node.type !== "team";
            //},
            "contextmenu": {
                items: treeContextMenu
            },
            "plugins": ["conditionalselect", "sort", "wholerow", "unique", "types", "contextmenu"]
        }
    );
});

function treeContextMenu(node) {

    var items = {
        addStudent: {
            label: "Добавить студента",
            action: function() {
                $("#m_SaveStudentButton").hide();
                $("#m_AddStudentButton").show();
                resetModalDialogsFields();
                loadTeamsInStudentModalDialog();
                $("#EditStudentDialog").modal('show');
            }
        },
        addTeam: {
            label: "Добавить взвод",
            action: function () {
                $("#m_SaveTeamButton").hide();
                $("#m_AddTeamButton").show();
                resetModalDialogsFields();
                $("#EditTeamDialog").modal('show');
            }
        },
        update: {
            label: "Изменить",
            action: function (data) {
                var parameter = {id: jstreeSelectedItem.id};
                if (jstreeSelectedItem.type === "student") {
                    $.getJSON("api/v1/student", parameter, onStudentEditClicked);
                    $("#m_SaveStudentButton").show();
                    $("#m_AddStudentButton").hide();
                    $('#EditStudentDialog').modal('show');
                }
                if (jstreeSelectedItem.type === "team") {
                    $.getJSON("api/v1/team", parameter, onTeamEditClicked);
                    $("#m_SaveTeamButton").show();
                    $("#m_AddTeamButton").hide();
                    $('#EditTeamDialog').modal('show');
                }
            }
        },
        remove: {
            label: "Удалить",
            action: function () {
                if (jstreeSelectedItem.type === "team") {
                    if (confirm("Удалить взвод? Будут удалены все студенты этого взвода...")) {
                        $.ajax({
                            url: "api/v1/team",
                            type: "DELETE",
                            data: {id: node.data.jstree.id},
                            success: function () {
                                location.reload();
                            },
                            statusCode: {
                                500: function (content) {
                                    alert("Необработанная ошибка на сервере. Обратитесь к разрабочтику \n\nТекст ошибки: " + content.responseText);
                                }
                            },
                            error: function (req, status, error) {
                                alert(error);
                            }
                        })
                    }
                }
                if (jstreeSelectedItem.type === "student") {
                    if (confirm("Удалить студента?")) {
                        $.ajax({
                            url: "api/v1/student",
                            type: "DELETE",
                            data: {id: node.data.jstree.id},
                            success: function () {
                                var nodeId = $("#BaseTree").jstree().get_selected()[0];
                                var nodeForDelete = $("#BaseTree").jstree().get_node(nodeId);
                                $("#BaseTree").jstree().delete_node(nodeForDelete);
                            },
                            statusCode: {
                                500: function (content) {
                                    alert("Необработанная ошибка на сервере. Обратитесь к разрабочтику \n\nТекст ошибки: " + content.responseText);
                                }
                            },
                            error: function (req, status, error) {
                                alert(error);
                            }
                        })
                    }
                }
            }
        }
    };

    if (node.type === "student") {
        delete items.addStudent;
        delete items.addTeam;
    }

    return items;
}

$("#BaseTree")
    .on("changed.jstree", function (e, data) {
        jstreeSelectedItem = {id: data.node.data.jstree.id, type: data.node.data.jstree.type};
        if (jstreeSelectedItem.type === "student") {
            var parameter = {id: jstreeSelectedItem.id};
            $.getJSON("api/v1/student", parameter, onStudentLoadedFromJsTreeChanged);
        }
        if (jstreeSelectedItem.type === "team") {
            $("#StudentContent").hide();
        }
    });

$('#m_SaveStudentButton').click(function () {
    addOrUpdateStudent("PUT");
});
$('#m_AddStudentButton').click(function () {
    addOrUpdateStudent("POST");
});

function addOrUpdateStudent(type) {
    var student = {
        id: jstreeSelectedItem.id,
        firstName: $('#m_FirstName').val(),
        lastName: $('#m_LastName').val(),
        middleName: $('#m_MiddleName').val(),
        group: $('#m_Group').val(),
        team: {id: $('#m_Team').val()}
    };
    $.ajax({
        url: "api/v1/student",
        type: type,
        contentType: "application/x-www-form-urlencoded; charset=UTF-8", //TODO: To JSON!
        success: function () {
            location.reload();
        },
        statusCode: {
            500: function (content) {
                alert("Необработанная ошибка на сервере. Обратитесь к разрабочтику \n\nТекст ошибки: " + content.responseText);
            }
        },
        error: function (req, status, error) {
            alert(error);
        },
        data: student
    })
}

$('#m_SaveTeamButton').click(function () {
    addOrUpdateTeam("PUT");
});
$('#m_AddTeamButton').click(function () {
    addOrUpdateTeam("POST");
});

function addOrUpdateTeam(type) {
    var team = {
        id: jstreeSelectedItem.id,
        number: $('#m_TeamNumber').val()
    };
    $.ajax({
        url: "api/v1/team",
        type: type,
        contentType: "application/x-www-form-urlencoded; charset=UTF-8", //TODO: To JSON!
        success: function () {
            location.reload();
        },
        statusCode: {
            500: function (content) {
                alert("Необработанная ошибка на сервере. Обратитесь к разрабочтику \n\nТекст ошибки: " + content.responseText);
            }
        },
        error: function (req, status, error) {
            alert(error);
        },
        data: team
    })
}

function onStudentEditClicked(view) {
    if (view.student == null) {
        return;
    }

    $("#m_LastName").val(view.student.lastName);
    $("#m_FirstName").val(view.student.firstName);
    $("#m_MiddleName").val(view.student.middleName);
    $("#m_Group").val(view.student.group);

    loadTeamsInStudentModalDialog(view);
}

function onTeamEditClicked(team) {
    $("#m_TeamNumber").val(team.number);
}

function onStudentLoadedFromJsTreeChanged(view) {

    if (view.student == null) {
        return;
    }

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

    $("#StudentContent").show();
}

function resetModalDialogsFields() {
    $("#m_LastName").empty();
    $("#m_FirstName").empty();
    $("#m_MiddleName").empty();
    $("#m_Group").empty();
    $("#m_Team").empty();
    $("#m_TeamNumber").empty();
}

function loadTeamsInStudentModalDialog(view) {
    $.getJSON("api/v1/teams", function (data) {
        onTeamsLoaded(data);
        if (jstreeSelectedItem.type === "student") {
            $("#m_Team").val(view.student.team.id);
        }
        if (jstreeSelectedItem.type === "team") {
            $("#m_Team").val(jstreeSelectedItem.id);
        }
    });
}

function onTeamsLoaded(view) {
    $("#m_Team").empty();
    $.each(view, function (index, value) {
        $("#m_Team").append('<option value="' + value.id + '">' + value.number + '</option>');
    });
}