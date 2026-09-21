unary_ops = {
    # Unary operators
    'UNOP_INC': '++',
    'UNOP_DCR': '--',
    'LUNOP_L_NOT': '!',
    'LUNOP_B_NOT': '~'
}

arithmetic_ops = {
    # Binary operators, in (sort of) precedence order
    'OP_MULT': '*',
    'OP_DIV': '/',
    'OP_REM': '%',

    'OP_ADD': '+',
    'OP_SUB': '-'
}

binary_ops = {
    'OP_B_AND': '&',
    'OP_B_OR': '|',
    'OP_B_XOR': '^',

    # Bitshift
    'OP_LSH': '<<',
    'OP_RSH': '>>'
}
logical_ops = {
    # Logical operators
    'OP_EQL': '==',
    'OP_L_AND': '&&',
    'OP_L_OR': '||',
    'OP_NEQ': '!=',
    'OP_LST': '<',
    'OP_GRT': '>',
    'OP_LSEQ': '<=',
    'OP_GREQ': '>='
}
