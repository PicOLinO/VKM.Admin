// Write your JavaScript code.

$(function () {
    $('#BaseTree').jstree(
        {
            "types": {
                "team": {
                    "icon": "glyphicon glyphicon-ok",
                    "hover_node": false,
                    "valid_children": ["student"]
                },
                "student": {
                    "icon": "glyphicon glyphicon-flash",
                    "valid_children": "none"
                }
            },
            "conditionalselect": function (node, event) {
                return node.type !== "team";
            },
            "plugins": ["conditionalselect", "sort", "wholerow", "unique", "types"]
        }
    );
});
$('#BaseTree')

    .on('changed.jstree', function (e, data) {
        $('#SelectedStudent').html(data.instance.get_node(data.selected[0]).text);
    })
    .on('loaded.jstree', function (e, data) {
        data.instance.set_type()
    });
