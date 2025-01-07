Feature: Pedido

Create new order

@pedido
Scenario: Successfully create a new order
    Given the customer has provided the order description 
    When the customer creates the order
    Then the order should be saved in the system