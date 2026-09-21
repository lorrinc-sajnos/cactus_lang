from ccts_types import *


tesfile_base = """
//TESTFILE
//{1}
//Generated on {2}

void print_i64(i64 num) $$C=> printf("%d\\n", num);
void print_u64(u64 num) $$C=> printf("%d\\n", num);


i32 main(){    
    {0}
    
    return 0;   
}
//ENDTESTFILE
"""

print_signed = """
    print_i64({0});
"""
print_unsigned = """
    print_u64({0});
"""

# 0: case name, #1: title, #2: op_code value,
# 3: type A, #4: A value
# 5: type B, #6: B value

# 7 result_type #8: result value

int_op_case = """
    //Case {0}: {3} {2} {5}
    //{1}: 
    {3} val_{0}_A = {4};
    {5} val_{0}_B = {6};
    
    //Expected value: {8}
    {7} test_result_{0} = val_{0}_A {2} val_{0}_B;
"""


def get_int_op_str(case_name: str, comment: str, op_code: str, 
                   type_a: IntType, value_a: str, 
                   type_b: IntType, value_b: str,
                   expected_val: str):    
    
    if type_a.type == PrimType.Signed:
        return_type = I64.as_code
    else:
        return_type = U64.as_code
    
    return int_op_case.format(case_name, comment, op_code,
                       type_a.as_code, value_a,
                       type_b.as_code, value_b,
                       return_type
                       )


def 


def generate_op_test_file(operator: str, folder: str):

    test_cases = ""

    counter = 0

    for type_a in INT_TYPES:
        for type_b in type_a.compatible_types:
            
            
            
