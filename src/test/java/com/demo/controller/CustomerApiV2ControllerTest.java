package com.demo.controller;

import com.demo.model.Customer;
import com.demo.repository.CustomerSpringDataRepository;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.mockito.Mockito;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.WebMvcTest;
import org.springframework.boot.test.mock.mockito.MockBean;
import org.springframework.http.MediaType;
import org.springframework.test.web.servlet.MockMvc;

import java.util.Arrays;
import java.util.List;

import static org.hamcrest.Matchers.hasSize;
import static org.hamcrest.Matchers.is;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.BDDMockito.given;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.get;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.*;

/**
 * Unit tests for {@link CustomerApiV2Controller} using {@link MockMvc}.
 */
@WebMvcTest(CustomerApiV2Controller.class)
class CustomerApiV2ControllerTest {

    @Autowired
    private MockMvc mockMvc;

    @MockBean
    private CustomerSpringDataRepository customerRepository;

    @Autowired
    private ObjectMapper objectMapper;

    @Test
    @DisplayName("GET /api/v2/customers returns list of customers")
    void testGetAllCustomers() throws Exception {
        // Arrange
        Customer cust1 = new Customer();
        cust1.setId(1L);
        cust1.setName("Alice");
        cust1.setEmail("alice@example.com");

        Customer cust2 = new Customer();
        cust2.setId(2L);
        cust2.setName("Bob");
        cust2.setEmail("bob@example.com");

        List<Customer> mockCustomers = Arrays.asList(cust1, cust2);
        given(customerRepository.findAll()).willReturn(mockCustomers);

        // Act & Assert
        mockMvc.perform(get("/api/v2/customers")
                        .accept(MediaType.APPLICATION_JSON))
                .andExpect(status().isOk())
                .andExpect(content().contentType(MediaType.APPLICATION_JSON))
                .andExpect(jsonPath("$", hasSize(2)))
                .andExpect(jsonPath("[0].id", is(1)))
                .andExpect(jsonPath("[0].name", is("Alice")))
                .andExpect(jsonPath("[0].email", is("alice@example.com")))
                .andExpect(jsonPath("[1].id", is(2)))
                .andExpect(jsonPath("[1].name", is("Bob")))
                .andExpect(jsonPath("[1].email", is("bob@example.com")));
    }
}
