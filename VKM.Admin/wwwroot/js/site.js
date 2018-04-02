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
            "conditionalselect": function (node) {
                return node.type !== "team";
            },
            "contextmenu": {
                items: treeContextMenu
            },
            "plugins": ["conditionalselect", "sort", "wholerow", "unique", "types", "contextmenu"]
        }
    );
});

function treeContextMenu(node) {

    var items = {
        add: {
            label: "Добавить",
            action: function() {
                //TODO:
            }
        },
        remove: {
            label: "Удалить",
            action: function () {
                if (confirm("Удалить студента?")) {
                    $.ajax({
                        url: "Home/RemoveStudent",
                        data: {id: node.data.jstree.id},
                        success: function() {
                            var nodeId = $("#BaseTree").jstree().get_selected()[0];
                            var nodeForDelete = $("#BaseTree").jstree().get_node(nodeId);
                            $("#BaseTree").jstree().delete_node(nodeForDelete);
                        },
                        statusCode: {
                            500: function (content) { alert("Необработанная ошибка на сервере. Обратитесь к разрабочтику \n\nТекст ошибки: " + content.responseText); }
                        },
                        error: function (req, status, errorObj) {
                            // handle status === "timeout"
                            // handle other errors
                        }
                    })
                }
            }
        },
        update: {
            label: "Изменить",
            action: function () {
                $('#EditStudentDialog').modal('show');
            }
        }
    };

    if (node.type === "student") {
        delete items.add;
    }

    return items;
}

$("#BaseTree")
    .on("changed.jstree", function (e, data) {
        var parameter = { id: data.node.data.jstree.id };
        $.getJSON("/Home/Student", parameter, onStudentLoaded);
    });

$('#EditStudentDialog')
    .on('show.bs.modal', function (e, data) {
        //TODO: Вызов ajax с получением данных по студенту. Заполнение модального диалога. + где-то тут callback по закрытию диалога (мб!)
    });

function onStudentLoaded(view) {

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

    var studentContentControl = document.getElementById("StudentContent");
    studentContentControl.style.visibility = "visible";
}
