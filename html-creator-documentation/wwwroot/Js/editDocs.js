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

// $('.new-item').click(function() {
//     const parentBlock = $(this).parent().parent()
//     const value = parentBlock.find('.value').text()
//     editDialog.find('[name=value]').val(value)
//     editDialog.dialog('open')
// })

$('.value').attr('contenteditable', 'true')
$('h2.title').attr('contenteditable', 'true')


GetArticleJson()
function GetArticleJson() {
    let json = []
    $('.article > .block-item').each(function() {
        json.push(GetElementJson($(this)))
    })
    console.log(JSON.stringify(json, undefined, 2))
}
function GetElementJson(element) {
    let json = {}

    const type = element.data('block-type')
    json.type = type

    const title = element.find('.block-title:first').text()
    json.title = title

    const innerElements = element.find(`> .block-item`)
    if (innerElements.length > 0) {
        json.elements = []
        innerElements.each(function() {
            json.elements.push(GetElementJson($(this)))
        })
    } else {
        switch (type) {
            case 'description':
            case 'tip':
                const value = element.find('.value:first').text()
                if (value != '') {
                    json.value = value
                }
                break
            case 'code':
            case 'list':
                json.listValues = []
                element.find('.value').each(function() {
                    json.listValues.push($(this).text())
                })
                break
        }
    }
    
    return json
}