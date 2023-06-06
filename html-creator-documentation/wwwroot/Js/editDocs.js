class SortableElement {
    constructor(container, sortableSelector) {
        this.container = container
        this.sortableSelector = sortableSelector
    }
}

const sortableElements = [
    new SortableElement('article', 'large-block'),
    new SortableElement('large-block', 'big-block'),
    new SortableElement('big-block', 'small-block'),
    new SortableElement('small-block', 'block')
]
sortableElements.forEach(sortableElement => {
    $(`.${sortableElement.sortableSelector}`).mouseenter(function(e) {
        e.preventDefault()
        e.stopPropagation()
        $(this).parents().removeClass('editable-block')
        $(this).addClass('editable-block')
    })
    $(`.${sortableElement.sortableSelector}`).mouseleave(function() {
        $(this).removeClass('editable-block')
    })
    $(`.${sortableElement.container}`).sortable({
        placeholder: 'block-placeholder',
        cursor: 'move',
        handle: `.move-button[data-target="${sortableElement.sortableSelector}"]`,
        items: `.${sortableElement.sortableSelector}`
    })
})
$('.empty').mouseenter(function(e) {
    e.preventDefault()
    e.stopPropagation()
    $(this).parent().addClass('editable-block')
})

const editDialog = $('#edit-dialog').dialog({
    autoOpen: false,
    height: 700,
    width: 1000,
    modal: true
})
var editValue

$('.edit-button').click(function() {
    const parentBlock = $(this).parent().parent()
    const value = parentBlock.find('.value').text()
    editDialog.find('[name=value]').val(value)
    editDialog.dialog('open')
})

$('.value').attr('contenteditable', 'true')
$('h2.title').attr('contenteditable', 'true')