package com.demo.repository;

import com.demo.model.Customer;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

/**
 * Spring Data JPA repository for {@link Customer} entities.
 */
@Repository
public interface CustomerRestRepository extends JpaRepository<Customer, Long> {
    // Additional query methods can be defined here if required.
}
